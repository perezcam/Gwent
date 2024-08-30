using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using UnityEngine;
    public class SemanticChecker : IVisitor
    {
        public List<string> errors = new List<string>();
        private Dictionary<ExpressionNode, string> typeTable = new Dictionary<ExpressionNode, string>();
        //methodParamsINOUT Key-NombredelMetodo Value-item1.Tipo de Entrada item2.Tipo de Salida
        private Dictionary<string, (string,string)> methodParamsINOUT = new Dictionary<string, (string,string)>();
        private Dictionary<string, List<(string,string)>> effectDebt = new Dictionary<string, List<(string,string)>>();
        private string[] possibleSources = {"\"board\"","\"hand\"","\"otherhand\"","\"deck\"","\"otherdeck\"","\"field\"","\"otherfield\"","\"parent\""};
        private ProgramNode program; 
        public SemanticChecker(ProgramNode program)
        {
            //Diccionario de Efectos INOUT Types
            methodParamsINOUT["Find"] = ("predicate","List<card>");
            methodParamsINOUT["Push"] = ("card","void");
            methodParamsINOUT["SendBottom"] = ("card","void");
            methodParamsINOUT["Pop"] = ("void", "card");
            methodParamsINOUT["Remove"] = ("card","void");
            methodParamsINOUT["Shuffle"] = ("void", "void");
            methodParamsINOUT["Add"] = ("card","void");
            methodParamsINOUT["FieldOfPlayer"] = ("player","List<card>");
            methodParamsINOUT["HandOfPlayer"] = ("player","List<card>");
            methodParamsINOUT["DeckOfPlayer"] = ("player","List<card>");
            methodParamsINOUT["GraveyardOfPlayer"] = ("player","List<card>");

            this.program = program;
            CheckSemantic(program);
        }
         public void CheckSemantic(ProgramNode program)
        {
            Scope ProgramScope = new Scope();
            ProgramScope.Declare("TriggerPlayer","player");
            Visit(program,ProgramScope);
        }
        public void Visit(ProgramNode node, Scope scope)
        {
            foreach (var statement in node.Statements)
            {
                statement.Accept(this,scope);
            }
        }
        public void Visit(EffectNode node,Scope scope)
        {
            if(node.Name is null)
                errors.Add($"El nombre del efecto no puede ser nulo: fila {node.Name.row}");
            if(node.Action is null)
                errors.Add($"El Bloque de Accion del efecto no debe ser null: fila {node.Action.target.row}");
            
            List<(string,string)> param_type = new List<(string, string)>();
            foreach (var param in node.Params)
            {
                param.Accept(this,scope);
                param_type.Add(((param.Variable as IdentifierNode)!.Name!,EvaluateExpressionType(param.Value!,scope)));
            }
            EvaluateLiteralExpresionNode(node.Name);
            effectDebt[(string)node.Name.Value] = param_type;
            Scope actionBlockScope = new Scope(scope);
            node.Action?.Accept(this,actionBlockScope);
        }
        public void Visit(CardNode node,Scope scope)
        {
            if (node.Name != null)
            {
               EvaluateLiteralExpresionNode(node.Name);
               typeTable[node.Name] = "card";
            }
            node.Power?.Accept(this,scope);
            node.Faction.Accept(this,scope);
            node.Type.Accept(this,scope);
            if((string)node.Type.Value != "Oro" && (string)node.Type.Value != "Plata" && (string)node.Type.Value != "Aumento" && (string)node.Type.Value != "Clima" )
                errors.Add($"Los tipos permitidos son Oro,Plata,Clima o Aumento y se recibio {node.Type.Value} en la fila {node.Type.row}");
            if((string)node.Faction.Value != "Humanity" && (string)node.Faction.Value != "Titans")
                errors.Add($"Las facciones permitidas son Humanity o Titans y se recibio {node.Faction.Value} en la fila {node.Faction.row}");
            foreach (string range in node.Range)
            {
                if(range != "\"Melee\"" && range != "\"Ranged\"" && range != "\"Siege\"" )
                errors.Add($"Los tipos permitidos son Melee,Ranged y Siege y se recibio {range}");
            }
            foreach (var activation in node.OnActivationBlock)
            {
                Scope activationScope = new Scope(scope);
                activation.Accept(this,activationScope);
            }
        }
        public void Visit(ActivationBlockNode node,Scope scope)
        {
            if(node.parent is null && node.selector is null)
                errors.Add($"El selector no puede ser nulo en este ambito: fila {node.selector.row}");
            else if(node.selector is null && node.parent is not null)
                node.selector = node.parent.selector;
            node.effect?.Accept(this,scope);
            node.selector?.Accept(this,scope);
            node.postAction?.Accept(this,scope);   
        }
        public void Visit(EffectBuilderNode node,Scope scope)
        {
            EvaluateLiteralExpresionNode(node.Name);
            if(!effectDebt.ContainsKey((string)node.Name.Value!))
            {
                errors.Add($"La carta esta haciendo referencia a un efecto no declarado: fila {node.Name.row}");
                return;
            }
            List<(string,string)> paramsDeclaration = new List<(string, string)>();
            foreach (var statement in node.assingments)
            {
                AssignmentNode assignment = (AssignmentNode)statement;
                string assignmentType = EvaluateExpressionType(assignment.Value!,scope);
                paramsDeclaration.Add(((assignment.Variable as IdentifierNode)!.Name!,assignmentType));
            }
            List<(string,string)> paramsDiffer = new List<(string, string)>();
            foreach (var param in effectDebt[(string)node.Name.Value!])
            {
                if(!paramsDeclaration.Contains(param))
                    paramsDiffer.Add(param);
            }
            if(paramsDiffer.Count != 0)
                errors.Add($"Los parametros {paramsDiffer} no coinciden con los que recibe el efecto {node.Name} en la fila: {node.assingments.First().row}");
        }
        public void Visit(SelectorNode node,Scope scope)
        {
            if(!possibleSources.ToList().Contains(node.source!.Name!))
                errors.Add($"La fuente elegida en Source no es aceptable: fila {node.source.row}");
            node.single?.Accept(this,scope);
            if(EvaluateExpressionType(node.single!,scope) != "bool")
                errors.Add($"La expresion {node.single} no es una expresion de tipo booleana: fila {node.single.row}");
            if(node.predicate != null)
            {
                Scope predicateScope = new Scope(scope);
                node.predicate?.Accept(this,predicateScope);

            }
        }
        public void Visit(PredicateFunction node,Scope scope)
        {
            if(scope.Vars.ContainsKey(node.target!.Name!))
                errors.Add($"La varibale {node.target.Name} ya existe: fila {node.target.row}");
            scope.Declare(node.target.Name!,"card");
            scope.typeInfo[node.target.Name!] = new CardInfo(node.target.Name!);
            node.filter?.Accept(this,scope);
        }
        public void Visit(ActionBlockNode node,Scope scope)
        {
            IdentifierNode context_id = (IdentifierNode) node.context.Variable!; 
            scope.typeInfo[context_id.Name!] = new ContextInfo(context_id.Name!); 
            string targetsName = (node.target.Variable as IdentifierNode)!.Name!;
            scope.Vars[targetsName] = "List<card>";
            scope.Vars[context_id.Name!] = "context";

            foreach (var statement in node.Statements)
            {
                if(statement is AssignmentNode assignment)
                {
                    assignment.Accept(this,scope);
                    continue;
                }
                Scope statementScope = new Scope(scope);
                statement.Accept(this,statementScope);
            }
        }
        public void Visit(ForStatementNode node,Scope scope)
        {
            if (node.Variable != null && node.Variable is IdentifierNode identifierNode &&!typeTable.ContainsKey(node.Variable))
            {
                typeTable[node.Variable] = "card";
                scope.Declare(identifierNode.Name!,"card");
                scope.typeInfo[identifierNode.Name!] = new CardInfo(identifierNode.Name!);
            }
            node.Body?.Accept(this,scope);
        }
        public void Visit(WhileStatementNode node,Scope scope)
        {
            if(node.Condition != null && node.Condition is BooleanBinaryExpressionNode || node.Condition is BooleanLiteralNode)
            {
                typeTable[node.Condition] = "bool";
                node.Condition.Accept(this,scope);
            }
            else 
                errors.Add($"La condicion recibida no es booleana: fila {node.Condition.row}");   
          
            node.Body?.Accept(this,scope);
           
        }
        public void Visit(BlockNode node,Scope scope)
        {
            foreach (var statement in node.Statements)
            {   
                if(statement is AssignmentNode)
                {
                    statement.Accept(this,scope);
                    continue;
                }
                Scope statementScope = new Scope(scope);
                statement.Accept(this,statementScope);
            }
        }
        public void Visit(AssignmentNode node,Scope scope)
        {
            node.Value?.Accept(this,scope);
            
            if (node.Variable is null)
                errors.Add($"Variable nula en la fila {node.Variable.row}");
            
            if(node.Variable is IdentifierNode identifier)
            {
                string varName = identifier.Name!;   
                scope.Declare(varName,EvaluateExpressionType(node.Value!,scope));            
            }
            else if(node.Variable is PropertyAccessNode property)
            {
                property.Accept(this,scope);
                if(EvaluateExpressionType(property,scope) != EvaluateExpressionType(node.Value!,scope))
                    errors.Add($"Se esta intentando modificar el valor de {property.PropertyName!.Name} con un tipo {EvaluateExpressionType(node.Value!,scope)} distinto al suyo fila: {property.PropertyName.row}");
            }
            else if(node.Variable is MethodCallNode method)
            {
                method.Accept(this,scope);
            }
            else
                errors.Add($"Tipo de Asignacion no esperada: fila {node.Variable.row}");
        }
        public void Visit(AccesExpressionNode node,Scope scope)
        {
            node.Expression?.Accept(this,scope);
        }
        public void Visit(IdentifierNode node,Scope scope)
        {
            if (node.Name != null && !scope.ContainsVar(node.Name))
            {
                errors.Add($"Variable '{node.Name}' is not declared: fila {node.row}");
            }
        }
        public void Visit(UnaryExpressionNode node,Scope scope)
        {
            node.Operand?.Accept(this,scope);
            string OperandID = (node.Operand as IdentifierNode)!.Name!;
            if(!scope.ContainsVar(OperandID))
                errors.Add($"Variable no declarada: fila {node.Operand.row}");
            if(scope.Resolve(OperandID) != "int")
                errors.Add($"No se puede incrementar una variable que no sea de tipo numerico: fila {node.Operand.row}");
        }
        public void Visit(MathBinaryExpressionNode node,Scope scope)
        {
            node.Left?.Accept(this,scope);
            node.Right?.Accept(this,scope);
            EvaluateExpressionType(node,scope);
        }
        public void Visit(BooleanLiteralNode node,Scope scope)
        {
            EvaluateExpressionType(node,scope);
        }
        public void Visit(BooleanBinaryExpressionNode node,Scope scope)
        {
            node.Left?.Accept(this,scope);
            node.Right?.Accept(this,scope);
            EvaluateExpressionType(node,scope);
        }
        public void Visit(NumberNode node,Scope scope)
        {
            EvaluateExpressionType(node,scope);
        }
        public void Visit(DataTypeNode node,Scope scope)
        {
            EvaluateExpressionType(node,scope);
        }
        public void Visit(StringNode node,Scope scope)
        {
            EvaluateExpressionType(node,scope);
            EvaluateLiteralExpresionNode(node);
        }
        public void Visit(PropertyAccessNode node,Scope scope)
        {
            node.Target?.Accept(this,scope);
            if(node.Target is PropertyAccessNode prop && !ContainsProperty(prop.PropertyName!,node.PropertyName!,scope))
            {
                errors.Add($"El objeto {prop.PropertyName!.Name} no contiene la propiedad {node.PropertyName.Name}: fila {node.PropertyName.row}");
            }
            else if (node.Target is IdentifierNode iden && (!ContainsProperty((node.Target as IdentifierNode)!, node.PropertyName!,scope) || !scope.ContainsVar((node.Target as IdentifierNode)!.Name!)))
            {
                errors.Add($"El objeto {(node.Target as IdentifierNode)!.Name } no contiene la propiedad {node.PropertyName.Name}: fila {node.PropertyName.row}");
            }    
        }
        public void Visit(CollectionIndexingNode node,Scope scope)
        {
            node.Collection?.Accept(this,scope);
            node.Index?.Accept(this,scope);
            if(EvaluateExpressionType(node.Index!, scope) != "int")
                errors.Add($"El valor de indexado no es correcto pues en de tipo {EvaluateExpressionType(node.Index!, scope)} en lugar de int: fila {node.Index.row}");
            if(EvaluateExpressionType(node.Collection!,scope) != "List<card>")
                errors.Add($"El objeto {node.Collection} no es indexable: fila {node.Collection.row}");

        }
        public void Visit(CompoundAssignmentNode node,Scope scope)
        {
            node.Variable?.Accept(this,scope);
            node.Value?.Accept(this,scope);
            if(EvaluateExpressionType(node.Variable!,scope) != EvaluateExpressionType(node.Value!,scope))
                errors.Add($"Los valores no son tipo entero ambos: fila {node.Variable.row}");
        }
        public void Visit(MethodCallNode node,Scope scope)
        {
            node.Target?.Accept(this,scope);
            if (node.Arguments != null)
            {
                foreach (var argument in node.Arguments)
                {
                    argument.Accept(this,scope);
                }
            }
            if(!methodParamsINOUT.ContainsKey(node.MethodName.Name))
                errors.Add($"El metodo {node.MethodName.Name} no es valido : fila {node.MethodName.row}");
            if(node.Arguments!.Count == 0 && methodParamsINOUT[node!.MethodName!.Name!].Item1 != "void")
                errors.Add($"El metodo {node.MethodName.Name} recibe argumentos que no han sido asignados: fila {node.MethodName.row}");
            else if(node.Arguments.Count!= 0 && methodParamsINOUT[node!.MethodName!.Name!].Item1 != EvaluateExpressionType(node.Arguments!.ElementAt(0),scope))
                errors.Add($"El metodo recibe {methodParamsINOUT[node!.MethodName!.Name!].Item1} y recibio {EvaluateExpressionType(node.Arguments!.ElementAt(0),scope)}: fila {node.Arguments.First()}");
            if(node.Target is null && methodParamsINOUT[node.MethodName.Name].Item1 != "player")
                errors.Add($"El metodo {node.MethodName.Name} debe ser aplicado a una coleccion: fila {node.MethodName.row}");
        }
        public void Visit(ConcatExpresion node,Scope scope)
        {
            node.right?.Accept(this,scope);
            node.left?.Accept(this,scope);
            EvaluateExpressionType(node,scope);
        }
        private string EvaluateExpressionType(ExpressionNode expr,Scope scope)
        {
            if (expr is IdentifierNode idNode)
            {
                if (scope.ContainsVar(idNode.Name!))
                {
                    return scope.Resolve(idNode.Name!)!;
                }
                else
                {
                    errors.Add($"La variable '{idNode.Name}' no esta declarada: fila {idNode.row}");
                }
            }
            else if (expr is NumberNode)
            {
                return "int";
            }
            else if (expr is StringNode)
            {
                return "string";
            }
            else if (expr is BooleanLiteralNode)
            {
                return "bool";
            }
            else if (expr is ConcatExpresion concExpr)
            {
                string leftType = EvaluateExpressionType(concExpr.left!, scope);
                string rightType = EvaluateExpressionType(concExpr.right!, scope);
                if (leftType == rightType && leftType == "string")
                {
                    EvaluateLiteralExpresionNode(concExpr);
                    return "string";
                }
                else if(leftType != "string")
                    errors.Add($"Se esperaba tipo string y se recibio {leftType} en {concExpr.left}: fila {concExpr.left.row}");
                else if(rightType != "string")
                    errors.Add($"Se esperaba tipo string y se recibio {rightType} en {concExpr.right}: fila {concExpr.right.row}");
            }
            else if (expr is MathBinaryExpressionNode mathExpr)
            {
                string leftType = EvaluateExpressionType(mathExpr.Left!, scope);
                string rightType = EvaluateExpressionType(mathExpr.Right!, scope);
                if (leftType == rightType && leftType == "int")
                {
                    return "int";
                }
                else if(leftType != "int")
                    errors.Add($"Se esperaba tipo int y se recibio {leftType} en {mathExpr.Left}: fila {mathExpr.Left.row}");
                else
                    errors.Add($"Se esperaba tipo int y se recibio {rightType} en {mathExpr.Right}: fila {mathExpr.Right.row}");
            }
            else if (expr is BooleanBinaryExpressionNode boolExpr)
            {
                string leftType = EvaluateExpressionType(boolExpr.Left!,scope);
                string rightType = EvaluateExpressionType(boolExpr.Right!,scope);
                if (leftType == rightType)
                {
                    return "bool";
                }
                else if(leftType != "bool" && (boolExpr.Operator == TokenType.Or || boolExpr.Operator == TokenType.And))
                    errors.Add($"Se esperaba tipo bool y se recibio {leftType} en {boolExpr.Left}: fila {boolExpr.Left.row}");
                else if(rightType != "bool" && (boolExpr.Operator == TokenType.Or || boolExpr.Operator == TokenType.And))
                    errors.Add($"Se esperaba tipo bool y se recibio {rightType} en {boolExpr.Right}: fila {boolExpr.Right.row}");
                else 
                    errors.Add($"El operador {boolExpr.Operator} compra dos expresiones del mismo tipo y la parte izquierda es {leftType} mientras que la derecha es {rightType}: fila {boolExpr.Left.row}");
               
            }
            else if (expr is PropertyAccessNode propertyAccess)
            {
                TypeInfo access = new CardInfo("empty");
                if(propertyAccess.Target is PropertyAccessNode prop)
                {
                    access = scope.ResolveTypeInfo(prop.PropertyName!.Name!)!;
                }
                else if(propertyAccess.Target is IdentifierNode id)
                {
                    access = scope.ResolveTypeInfo(id.Name!)!;
                }
                if(access is null || !access.Properties.ContainsKey(propertyAccess.PropertyName!.Name!))
                {
                    errors.Add($"La propiedad {propertyAccess.PropertyName!.Name!} no existe en {propertyAccess.Target}");
                }
                else
                    return access.Properties![propertyAccess.PropertyName!.Name!];
            }
            else if (expr is MethodCallNode method)
            {
                return methodParamsINOUT[method.MethodName!.Name!].Item2;
            }
            else if (expr is DataTypeNode typeNode)
            {
                if(typeNode.type == TokenType.NumberType)
                    return "int";
                else if(typeNode.type == TokenType.StringType)
                    return "string";
                else
                    return "bool";
            }
            else if(expr is UnaryExpressionNode unary)
            {
                return EvaluateExpressionType(unary.Operand!,scope);
            }
            else if(expr is CollectionIndexingNode)
            {
                return "card";
            }
            else if(expr is PredicateFunction)
            {
                return "predicate";
            }
            errors.Add($"La expresion {expr.GetType()} no coincide con ningun tipo esperado : fila {expr.row}");
            return null;
        }
        public bool ContainsProperty(ExpressionNode target, IdentifierNode property,Scope scope)
        {
            TypeInfo access = new CardInfo("fake");
            if(target is PropertyAccessNode prop)
            {
                access = scope.ResolveTypeInfo(prop.PropertyName!.Name!)!;
            }
            else if(target is IdentifierNode id)
            {
                access = scope.ResolveTypeInfo(id.Name!)!;
            }
            else if(target is MethodCallNode method)
            {
                throw new NotImplementedException();
            }
            if(access is null || !access.Properties!.ContainsKey(property.Name!))
                return false;
            return true;
        }
        public static void EvaluateLiteralExpresionNode(ExpressionNode expression)
        {
            if(expression is StringNode)
                expression.Value = (expression.Value as String).Trim('/').Trim().Trim('"');
            else if(expression is ConcatExpresion expr)
            {
                EvaluateLiteralExpresionNode(expr.left);
                EvaluateLiteralExpresionNode(expr.right);
                
                if(expr.IsComp)
                    expr.Value = (string)expr.left.Value + " " + (string)expr.right.Value;
                else
                    expr.Value = (string)expr.left.Value + (string)expr.right.Value;
            }
        }
    }