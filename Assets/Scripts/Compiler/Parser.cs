using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.XPath;
using GameLogic;
using Interpeter;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.ShortcutManagement;
using UnityEditor.XR;
using UnityEngine;

public abstract class ASTNode{
    public abstract void Accept(IVisitor visitor,Scope scope);
    public int row{get;set;}
    public int priority{get;set;}
}
public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements = new List<ASTNode>();
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class EffectNode : ASTNode
{
    public EffectNode()
    {
        priority = 1;
    } 
    public ExpressionNode Name { get; set; }
    public List<AssignmentNode> Params = new List<AssignmentNode>();
    public ActionBlockNode Action {get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class CardNode : ASTNode
{
    public CardNode()
    {
        priority = 0;
    }
    public ExpressionNode Name { get; set; } 
    public ExpressionNode Type { get; set; }
    public List<string> Effect  = new List<string>();
    public ExpressionNode Faction{get;set;}
    public ExpressionNode Power{get;set;}
    public List<string> Range = new List<string>();
    public List<ActivationBlockNode> OnActivationBlock = new List<ActivationBlockNode>();
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class ActivationBlockNode: ASTNode
{
    public ActivationBlockNode(ActivationBlockNode parent = null)
    {   
        this.parent = parent!;
    }
    public ActivationBlockNode parent;
    public EffectBuilderNode effect {get;set;}
    public SelectorNode selector {get;set;} 
    public ActivationBlockNode postAction {get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class ErrorBlockNode
{
    public string Message { get; }
    public List<Token> Tokens { get; }

    public ErrorBlockNode(string message, List<Token> tokens)
    {
        Message = message;
        Tokens = tokens;
    }

    public override string ToString()
    {
        return $"{Message}";
    }
}

public class EffectBuilderNode:StatementNode
{
    public ExpressionNode Name{get;set;}
    public List<StatementNode> assingments{get;set;} = new List<StatementNode>();
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class SelectorNode:StatementNode
{
    public IdentifierNode source{get;set;}
    public ExpressionNode single{get;set;}
    public PredicateFunction predicate{get;set;} 
    public List<Card> targets{get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class PredicateFunction: ExpressionNode
{
    public IdentifierNode target {get;set;}
    public ExpressionNode filter {get;set;}
    public string propertyName{get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class ActionBlockNode : ASTNode
{
    public List<StatementNode> Statements { get; } = new List<StatementNode>();
    public AssignmentNode target = new AssignmentNode();
    public AssignmentNode context = new AssignmentNode();
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public abstract class StatementNode : ASTNode {}
public class ForStatementNode : StatementNode
{
    public ExpressionNode Variable { get; set; }
    public ExpressionNode Collection { get; set; }
    public BlockNode Body { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class WhileStatementNode : StatementNode
{
    public ExpressionNode Condition { get; set; }
    public BlockNode Body { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class AssignmentNode : StatementNode
{
    public ExpressionNode Variable { get; set;}
    public TokenType  type{get;set;}
    public ExpressionNode Value { get; set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public abstract class ExpressionNode : ASTNode {
    public Evaluate.PropertyReference ReferencedObject {get;set;}
    public object Value{get;set;}
}
public class AccesExpressionNode : StatementNode
{
    public ExpressionNode Expression { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}

public class IdentifierNode : ExpressionNode
{
    public IdentifierNode(string name)
    {
        Name = name;
    }
    public string Name {get; set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class UnaryExpressionNode : ExpressionNode
{
    public TokenType Operator {get;set;}
    public ExpressionNode Operand {get;set;}
    public bool Atend {get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class MathBinaryExpressionNode : ExpressionNode
{
    public static Dictionary<TokenType, int> Levels = new Dictionary<TokenType, int>
    {
        { TokenType.Plus, 2 },
        { TokenType.Minus, 2 },
        { TokenType.Multiply, 3 },
        { TokenType.Divide, 3 },
        { TokenType.And, 1 },
        { TokenType.Or, 1 },
        { TokenType.EqualValue,2},
        { TokenType.NotEquals, 2 },
        { TokenType.LessThan, 2 },
        { TokenType.GreaterThan, 2 },
        { TokenType.LessThanOrEquals, 2 },
        { TokenType.GreaterThanOrEquals, 2 }
    };
    public ExpressionNode Left { get; set; }
    public TokenType Operator { get; set; }
    public ExpressionNode Right { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class BooleanLiteralNode : ExpressionNode
{
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}

public class BooleanBinaryExpressionNode : ExpressionNode
{
    public ExpressionNode Left { get; set; }
    public TokenType Operator { get; set; }
    public ExpressionNode Right { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class NumberNode : ExpressionNode
{
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class DataTypeNode: ExpressionNode
{
    public TokenType type {get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class StringNode : ExpressionNode
{
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}

public class BlockNode : ASTNode
{
    public List<StatementNode> Statements { get; } = new List<StatementNode>();
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class PropertyAccessNode : ExpressionNode
{
    public ExpressionNode Target { get; set;}
    public IdentifierNode PropertyName { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class CollectionIndexingNode : ExpressionNode
{
    public ExpressionNode Collection { get; set; }
    public ExpressionNode Index { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }

}
public class CompoundAssignmentNode : AssignmentNode
{
    public TokenType Operator { get; set; }
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class MethodCallNode : ExpressionNode
{
    public ExpressionNode Target {get;set;}
    public IdentifierNode MethodName {get;set;}
    public List<ExpressionNode> Arguments {get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class ConcatExpresion: ExpressionNode
{
    public ExpressionNode right{get;set;}
    public ExpressionNode left {get;set;}
    public bool IsComp{get;set;}
    public override void Accept(IVisitor visitor,Scope scope)
    {
        visitor.Visit(this,scope);
    }
}
public class Parser
{
    private List<Token> tokens;
    private int position;
    public List<ErrorBlockNode> errorList = new List<ErrorBlockNode>();
    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
        this.position = 0;
    }
    private Token CurrentToken => position < tokens.Count ? tokens[position] : null;
    private Token GetNextToken()
    {
        if(position+1 < tokens.Count)
            return tokens[position+1];
        return null;
    }    
    private Token GetLastToken()
    {
        if(position-1>0)
            return tokens[position-1];
        return null;
    }
    private Token Match(TokenType type)
    {
        Token token = CurrentToken;
        if (token != null && token.Type == type)
        {
            position++;
            return token;
        }
        else
        {   
            if(CurrentToken is null)
                errorList.Add(new ErrorBlockNode($"Esperaba un {type} en la linea {GetLastToken()?.Row} pero no recibio nada",new List<Token>{token}));
            else
                errorList.Add(new ErrorBlockNode($"Esperaba un {type} en la linea {CurrentToken?.Row} pero recibio  \"{token.Type}\"",new List<Token>{token}));
            return token;
        }
    }
    public ProgramNode Parse()
    {
        ProgramNode program = new ProgramNode();
        bool activeError = false;
        while (CurrentToken != null)
        {
            if (CurrentToken.Type == TokenType.Effect)
            {
                program.Statements.Add(ParseEffect());
                activeError = false;
            }
            else if (CurrentToken.Type == TokenType.Card)
            {
                program.Statements.Add(ParseCard());
                activeError = false;
            }
            else
            {
                if(!activeError)
                {
                    errorList.Add(new ErrorBlockNode($"Recibio {CurrentToken.Type} pero esperaba effect o card en la linea {CurrentToken.Row}",new List<Token>{CurrentToken}));
                    activeError = true;
                }
                else 
                    errorList.Last().Tokens.Add(CurrentToken);
                Match(CurrentToken.Type);
            }
        }
        program.Statements = program.Statements.OrderByDescending((x)=>x.priority).ToList();
        return program;
    }
    private EffectNode ParseEffect()
    {
        EffectNode effect = new EffectNode();
        Match(TokenType.Effect);
        Match(TokenType.OpenBrace);
        bool activeError = false;
        while (CurrentToken != null && CurrentToken!.Type != TokenType.CloseBrace)
        {
            Token propertyToken = CurrentToken;
            switch (propertyToken.Type)
            {
                case TokenType.Name:
                    Match(TokenType.Name);
                    Match(TokenType.Colon);
                    effect.Name = ParseExpression();
                    effect.Name.row = GetLastToken().Row;
                    Match(TokenType.Comma);
                    activeError = false;
                    break;

                case TokenType.Params:
                    Match(TokenType.Params);
                    Match(TokenType.Colon);
                    Match(TokenType.OpenBrace);
                    while (CurrentToken != null && CurrentToken.Type != TokenType.CloseBrace)
                    {
                        effect.Params.Add((AssignmentNode)ParseAssignment());
                        effect.Params.Last().row = GetLastToken().Row;
                    }
                    Match(TokenType.CloseBrace);
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                
                case TokenType.Action:
                    Match(TokenType.Action);
                    Match(TokenType.Colon);
                    effect.Action = ParseActionBlock();
                    effect.Action.row = GetLastToken().Row;
                    activeError = false;
                    break;

                default:
                {
                    if(!activeError)       
                    {
                        errorList.Add(new ErrorBlockNode($"Token inesperado en las valores del objeto effect en la fila {CurrentToken.Row} recibio {CurrentToken}",new List<Token>{CurrentToken}));
                        activeError = true;
                    }      
                    else
                        errorList.Last().Tokens.Add(CurrentToken);
                    Match(CurrentToken.Type);
                    break;
                }  
            }
            // si se acaban los token y no cerraron la llave
            if(CurrentToken is null)
                break;
        }
        Match(TokenType.CloseBrace); 
        return effect;
    }
    private CardNode ParseCard()
    {
        CardNode card = new CardNode();
        Match(TokenType.Card);
        Match(TokenType.OpenBrace);
        bool activeError = false;
        while (CurrentToken != null && CurrentToken!.Type != TokenType.CloseBrace)
        {
            Token propertyToken = CurrentToken;
            switch (propertyToken.Type)
            {
                case TokenType.Name:
                    Match(TokenType.Name);
                    Match(TokenType.Colon);
                    card.Name = ParseExpression();
                    card.Name.row = GetLastToken().Row;
                    Match(TokenType.Comma);
                    activeError = false;
                    break;

                case TokenType.Type:
                    Match(TokenType.Type);
                    Match(TokenType.Colon);
                    card.Type = ParseExpression();
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                
                case TokenType.EffectKeyword:
                    Match(TokenType.EffectKeyword);
                    Match(TokenType.Colon);
                    card.Effect.Add(Match(TokenType.String).Value);
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                case TokenType.Range:
                    Match(TokenType.Range);
                    Match(TokenType.Colon);
                    Match(TokenType.OpenBracket);
                    while(CurrentToken.Type != TokenType.CloseBracket)
                    {
                        card.Range.Add(Match(TokenType.String).Value);
                        if(CurrentToken.Type != TokenType.CloseBracket)
                            Match(TokenType.Comma);
                    }
                    Match(TokenType.CloseBracket);
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                case TokenType.Power:
                    Match(TokenType.Power);
                    Match(TokenType.Colon);
                    card.Power = ParseExpression();
                    card.Power.row = GetLastToken().Row;
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                case TokenType.Faction:
                    Match(TokenType.Faction);
                    Match(TokenType.Colon);
                    card.Faction = ParseExpression();
                    Match(TokenType.Comma);
                    activeError = false;
                    break;
                case TokenType.OnActivation:
                    Match(TokenType.OnActivation);
                    Match(TokenType.Colon);
                    Match(TokenType.OpenBracket);
                    while(CurrentToken.Type != TokenType.CloseBracket)
                    {
                        card.OnActivationBlock.Add(ParseActivationBlock());
                        card.OnActivationBlock.Last().row = GetLastToken().Row;
                        if(CurrentToken.Type == TokenType.Comma)
                            Match(TokenType.Comma);
                    }
                    Match(TokenType.CloseBracket);
                    activeError = false;
                    break;
                default:
                {
                    if(!activeError)  
                    {
                        errorList.Add(new ErrorBlockNode($"Token inesperado en las valores del objeto card en la fila {CurrentToken.Row} recibio {CurrentToken}",new List<Token>{CurrentToken}));
                        activeError = true;
                    }           
                    else
                        errorList.Last().Tokens.Add(CurrentToken);
                    Match(CurrentToken.Type);
                    break;
                }    
            }
        }
        Match(TokenType.CloseBrace);
        return card;
    }
    private ActivationBlockNode ParseActivationBlock(ActivationBlockNode parent = null)
    {
        ActivationBlockNode actionBlock = new ActivationBlockNode(parent);
        Match(TokenType.OpenBrace);
        bool activeError = false;
        while(CurrentToken != null && CurrentToken?.Type != TokenType.CloseBrace)
        {
            if(CurrentToken?.Type == TokenType.EffectKeyword)
            {
                actionBlock.effect = ParseEffectBuilderNode();
                actionBlock.effect.row = GetLastToken().Row;
                activeError = false;
            }
            else if(CurrentToken?.Type == TokenType.Selector)
            {
                actionBlock.selector = ParseSelectorNode();
                actionBlock.selector.row = GetLastToken().Row;
                activeError = false;
            }
            else if(CurrentToken?.Type == TokenType.PostAction)
            {
                Match(TokenType.PostAction);
                Match(TokenType.Colon);
                actionBlock.postAction = ParseActivationBlock(actionBlock);
                actionBlock.postAction.row = GetLastToken().Row;
                activeError = false;
            }
            else
            {
                if(!activeError)   
                {
                    errorList.Add(new ErrorBlockNode($"Esperaba EffectKW,Selector o PostAction pero recibio {CurrentToken?.Value} en  la fila {CurrentToken.Row}",new List<Token>{CurrentToken}));
                    activeError = true;
                }          
                else
                    errorList.Last().Tokens.Add(CurrentToken);
                Match(CurrentToken.Type);
                break;
            }
        }
        Match(TokenType.CloseBrace);
        return actionBlock;
    }
    private EffectBuilderNode ParseEffectBuilderNode()
    {
        EffectBuilderNode effectBuild = new EffectBuilderNode();
        Match(TokenType.EffectKeyword);
        Match(TokenType.Colon);
        //Sintactic Sugar
        if(CurrentToken != null)
        {  
            if(CurrentToken?.Type == TokenType.String)
            {
                effectBuild.Name = ParseExpression();
                effectBuild.Name.row = GetLastToken().Row;
                Match(TokenType.Comma);
                return effectBuild;
            }
            Match(TokenType.OpenBrace);
            while(CurrentToken != null && CurrentToken?.Type != TokenType.CloseBrace)
            {
                if(CurrentToken?.Type == TokenType.Name)
                {
                    Match(TokenType.Name);
                    Match(TokenType.Colon);
                    effectBuild!.Name = ParseExpression();
                    effectBuild.Name.row = GetLastToken().Row;
                    Match(TokenType.Comma);
                }
                else
                {
                    effectBuild.assingments.Add(ParseAssignment());
                    effectBuild.assingments.Last().row = GetLastToken().Row;
                }
            }
        }
        Match(TokenType.CloseBrace);
        Match(TokenType.Comma);
        return effectBuild!;
    }
    private SelectorNode ParseSelectorNode()
    {
        SelectorNode selector = new SelectorNode();
        Match(TokenType.Selector);
        Match(TokenType.Colon);
        Match(TokenType.OpenBrace);
        while(CurrentToken != null && CurrentToken?.Type != TokenType.CloseBrace)
        {
            if(CurrentToken?.Type == TokenType.Source)
            {
                Match(TokenType.Source);
                Match(TokenType.Colon);
                IdentifierNode source = new IdentifierNode(Match(CurrentToken.Type).Value);
                selector.source = source;
                Match(TokenType.Comma);
            }
            else if(CurrentToken?.Type == TokenType.Single)
            {
                Match(TokenType.Single);
                Token colon = Match(TokenType.Colon);
                selector.single = ParseExpression();
                selector.single.row = GetLastToken().Row;
                if (selector.single is not BooleanBinaryExpressionNode && selector.single is not BooleanLiteralNode)
                    errorList.Add(new ErrorBlockNode($"Esperaba una expresion boolena en el single del selector de la carta y recibio {selector.single.GetType()} en ParseSelectorNode_Single en la fila {colon.Row} ",new List<Token>{colon,CurrentToken})); 
                Match(TokenType.Comma);
            }
            else if(CurrentToken?.Type == TokenType.Predicate)
            {
                PredicateFunction predicate = new PredicateFunction();
                Match(TokenType.Predicate);
                Match(TokenType.Colon);
                Match(TokenType.OpenParen);
                IdentifierNode cardID = new IdentifierNode(Match(CurrentToken!.Type).Value); 
                predicate.target = cardID;
                Match(TokenType.CloseParen);
                Match(TokenType.Arrow);
                Token begPredicate = CurrentToken;
                predicate.filter = ParseExpression();
                predicate.filter.row = GetLastToken().Row;
                if (predicate.filter is not BooleanBinaryExpressionNode && selector.single is not BooleanLiteralNode)
                    errorList.Add(new ErrorBlockNode($"Esperaba una expresion boolena en el single del selector de la carta y recibio {predicate.filter.GetType()} en ParseSelectorNode_Single en la fila {begPredicate.Row} ",new List<Token>{begPredicate,CurrentToken})); 
                selector.predicate = predicate;
            }
        }
        Match(TokenType.CloseBrace);
        Match(TokenType.Comma);
        return selector;
    }
    private ActionBlockNode ParseActionBlock()
    {
        ActionBlockNode actionBlock = new ActionBlockNode();
        Match(TokenType.OpenParen);
        IdentifierNode target = new IdentifierNode(Match(TokenType.Identifier).Value);
        actionBlock.target.Variable = target;
        Match(TokenType.Comma);
        IdentifierNode context = new IdentifierNode(Match(TokenType.Identifier).Value);
        actionBlock.context.Variable = context;
        Match(TokenType.CloseParen);
        Match(TokenType.Arrow);
        
        Match(TokenType.OpenBrace);
        while (CurrentToken != null && CurrentToken.Type != TokenType.CloseBrace)
        {
            actionBlock.Statements.Add(ParseStatement());
            actionBlock.Statements.Last().row = GetLastToken().Row;
        }
        Match(TokenType.CloseBrace);
        return actionBlock;
    }
    private StatementNode ParseStatement()
    {
        TokenType nextToken = GetNextToken()?.Type ?? TokenType.Unknown;
        if (CurrentToken?.Type == TokenType.For)
        {
            return ParseForStatement();
        }
        else if (CurrentToken?.Type == TokenType.While)
        {
            return ParseWhileStatement();
        }
        else if (nextToken == TokenType.Equals || nextToken == TokenType.PlusEqual || nextToken == TokenType.MinusEqual || nextToken == TokenType.MultiplyEqual || nextToken == TokenType.DivideEqual)
        {
            return ParseAssignment();
        }
        else if (CurrentToken?.Type == TokenType.Identifier)
        {
            // Logica para una llamada de metodo,propiedad o expresion unaria
            Token begExpr = CurrentToken;
            ExpressionNode expr = ParseExpression();
            if (expr is MethodCallNode)
            {
                Match(TokenType.DotCom);
                return new AccesExpressionNode { Expression = expr };
            }
            else if ( expr is PropertyAccessNode || expr is CollectionIndexingNode)
            {   
                if(IsAssignmentOperator(CurrentToken.Type))
                {
                    Token op = CurrentToken;
                    Match(op.Type);
                    StatementNode assingment;
                    if(op.Type ==  TokenType.Equals)
                    {
                        assingment = new AssignmentNode
                        {
                            Variable = expr,
                            Value = ParseExpression(),
                        };
                    }
                    else 
                    {
                        assingment = new CompoundAssignmentNode
                        {
                            Variable = expr,
                            Operator = op.Type,
                            Value = ParseExpression()
                        };
                    }

                    Match(TokenType.DotCom);
                    assingment.row = GetLastToken().Row;
                    return assingment;
                }
                Match(TokenType.DotCom);
                return new AccesExpressionNode{ Expression = expr };
            }
            else if(expr is UnaryExpressionNode)
            {
                Match(TokenType.DotCom);
                return new AccesExpressionNode { Expression = expr };
            }
            else
            {
                errorList.Add(new ErrorBlockNode($"Se esperaba un Metodo o una propiedad, o una expresion unaria en la fila {begExpr.Row}",new List<Token>{begExpr}));
                Match(CurrentToken.Type);
                return null;
            }
        }
        else
        {
            errorList.Add(new ErrorBlockNode($"Se esperaba un statement en la fila {CurrentToken.Row}",new List<Token>{CurrentToken}));
            Match(CurrentToken.Type);
            return null;
        }
    }

    private ForStatementNode ParseForStatement()
    {
        ForStatementNode forStatement = new ForStatementNode();
        Match(TokenType.For);
        Match(TokenType.OpenParen);
        forStatement.Variable = ParseExpression();
        forStatement.Variable.row = GetLastToken().Row;
        Match(TokenType.In);
        forStatement.Collection = ParseExpression();
        forStatement.Collection.row = GetLastToken().Row;
        Match(TokenType.CloseParen);
        forStatement.Body = ParseBlock();
        forStatement.Body.row = GetLastToken().Row;
        return forStatement;
    }
    private WhileStatementNode ParseWhileStatement()
    {
        WhileStatementNode whileStatement = new WhileStatementNode();
        Match(TokenType.While);
        Match(TokenType.OpenParen);
        whileStatement.Condition = ParseExpression();
        whileStatement.Condition.row = GetLastToken().Row;
        Match(TokenType.CloseParen);
        whileStatement.Body = ParseBlock();
        whileStatement.Body.row = GetLastToken().Row;
        return whileStatement;
    }
    private StatementNode ParseAssignment()
    {
        Token befExpr = CurrentToken;
        ExpressionNode left = ParsePrimaryExpression();
        left.row = GetLastToken().Row;
        Token afterExpr = CurrentToken == null?befExpr : CurrentToken;

        if (left is IdentifierNode || left is PropertyAccessNode)
        {
            Token op = CurrentToken;
            if (IsAssignmentOperator(op.Type))
            {
                Match(op.Type);
                ExpressionNode right = ParseExpression();
                right.row = GetLastToken().Row;
                if (op.Type == TokenType.Equals)
                {
                    Match(TokenType.DotCom);
                    return new AssignmentNode
                    {
                        Variable = left,
                        Value = right
                    };
                }
                else if(op.Type == TokenType.Colon)
                {
                    Match(TokenType.Comma);
                    return new AssignmentNode
                    {
                        Variable = left,
                        Value = right
                    };
                }
                else
                {
                    Match(TokenType.Comma);
                    return new CompoundAssignmentNode
                    {
                        Variable = left,
                        Operator = GetSimpleOperator(op),
                        Value = right
                    };
                }
            }
            else
            {
                Token start = CurrentToken;
                Match(CurrentToken.Type);
                ExpressionNode right = ParseExpression();
                errorList.Add(new ErrorBlockNode($"Operador de asignacion esperado en la linea {start.Row} lo q recibio fue {start.Value}",new List<Token>{CurrentToken,CurrentToken}));
                return null;
            }
            
        }
        else
        {
            errorList.Add(new ErrorBlockNode($"Asignacion no valida en la linea {befExpr.Row} lo q recibio fue {left.GetType()}",new List<Token>{befExpr,afterExpr} ));
            return null;
        }
    }

    private ExpressionNode ParseExpression(int precedence = 0)
    {
        ExpressionNode left = ParsePrimaryExpression();
        left.row = GetLastToken().Row;
        while (CurrentToken != null && IsOperator(CurrentToken.Type) && MathBinaryExpressionNode.Levels[CurrentToken.Type] > precedence)
        {
            TokenType op = CurrentToken.Type;
            Match(op);
            ExpressionNode right = ParseExpression(MathBinaryExpressionNode.Levels[op]);
            right.row = GetLastToken().Row;

            if (IsBooleanOperator(op))
            {
                left = new BooleanBinaryExpressionNode { Left = left, Operator = op, Right = right };
            }
            else
            {
                left = new MathBinaryExpressionNode { Left = left, Operator = op, Right = right };
            }
        }
        return left;
    }

    private bool IsAssignmentOperator(TokenType type)
    {
        return type == TokenType.Colon ||type == TokenType.Equals || type == TokenType.PlusEqual || type == TokenType.MinusEqual || type == TokenType.MultiplyEqual || type == TokenType.DivideEqual;
    }

    private TokenType GetSimpleOperator(Token compoundOperator)
    {
        switch (compoundOperator.Type)
        {
            case TokenType.PlusEqual: return TokenType.Plus;
            case TokenType.MinusEqual: return TokenType.Minus;
            case TokenType.MultiplyEqual: return TokenType.Multiply;
            case TokenType.DivideEqual: return TokenType.Divide;
            default: 
            {
                errorList.Add(new ErrorBlockNode($"Operacion Compuesta Invalida, se recibio {compoundOperator.Value} en la linea {compoundOperator?.Row}",new List<Token>{compoundOperator}));
                return compoundOperator.Type;
            }   
        }
    }

    private bool IsBooleanOperator(TokenType type)
    {
        return type == TokenType.And || type == TokenType.Or || type == TokenType.Equals || type == TokenType.NotEquals || type == TokenType.EqualValue||
            type == TokenType.LessThan || type == TokenType.GreaterThan || type == TokenType.LessThanOrEquals || type == TokenType.GreaterThanOrEquals;
    }
    private bool IsOperator(TokenType type)
    {
        return MathBinaryExpressionNode.Levels.ContainsKey(type);
    }
    private ExpressionNode ParsePrimaryExpression()
    {
        //Parseo de Acceso a Propiedad o Metodo
        ExpressionNode expr = ParseBasicPrimaryExpression();
        //Parsear Predicados en Metodos
        if(CurrentToken?.Type == TokenType.OpenParen)
        {
            Match(TokenType.OpenParen);
            ExpressionNode argument = ParseExpression();
            Match(TokenType.CloseParen);
            expr = new MethodCallNode{Target = null, MethodName = expr as IdentifierNode, Arguments = new List<ExpressionNode>{argument}};

        }
        while (CurrentToken != null && CurrentToken.Type == TokenType.Dot)
        {
            Match(TokenType.Dot);
            IdentifierNode propertyName = (IdentifierNode)ParseBasicPrimaryExpression();
            propertyName.row = GetLastToken().Row;
            if (CurrentToken?.Type == TokenType.OpenParen)
            {
                Match(TokenType.OpenParen);
                if(propertyName.Name == "Find")
                {
                    PredicateFunction predicate = new PredicateFunction();
                    Match(TokenType.OpenParen);
                    IdentifierNode objectID = new IdentifierNode(Match(CurrentToken!.Type).Value); 
                    objectID.row = GetLastToken().Row;
                    predicate.target = objectID;
                    Match(TokenType.CloseParen);
                    Match(TokenType.Arrow);
                    Token begExpr = CurrentToken;
                    predicate.filter = ParseExpression();
                    predicate.filter.row = GetLastToken().Row;
                    if (predicate.filter is not BooleanBinaryExpressionNode)
                        errorList.Add(new ErrorBlockNode($"Esperaba una expresion boolena en el predicado de la fila {begExpr.Row} y recibio {predicate.filter.GetType()}",new List<Token>{begExpr}));
                    expr = new MethodCallNode { Target = expr, MethodName = propertyName, Arguments =new List<ExpressionNode> {predicate} };
                    Match(TokenType.CloseParen);
                    continue;
                }
                List<ExpressionNode> arguments = new List<ExpressionNode>();
                if (CurrentToken?.Type != TokenType.CloseParen)
                {
                    do
                    {
                        arguments.Add(ParseExpression());
                        arguments.Last().row = GetLastToken().Row;
                    } 
                    while(CurrentToken?.Type != TokenType.CloseParen);
                }
                Match(TokenType.CloseParen);
                expr = new MethodCallNode { Target = expr, MethodName = propertyName, Arguments = arguments };
            }
            else
            {
                expr = new PropertyAccessNode { Target = expr, PropertyName = propertyName };
            }
            if(CurrentToken?.Type == TokenType.OpenBracket)
            {
                CollectionIndexingNode Indexing = new CollectionIndexingNode();
                Match(TokenType.OpenBracket);
                Indexing.Collection = expr;
                Indexing.Collection.row = GetLastToken().Row;
                Indexing.Index = ParseExpression();
                Indexing.Index.row = GetLastToken().Row;
                Match(TokenType.CloseBracket);
                expr = Indexing;
            }
        }
        if(CurrentToken?.Type == TokenType.OpenBracket)
        {
            CollectionIndexingNode Indexing = new CollectionIndexingNode();
            Match(TokenType.OpenBracket);
            Indexing.Collection = expr;
            Indexing.Collection.row = GetLastToken().Row;
            Indexing.Index = ParseExpression();
            Indexing.Index.row = GetLastToken().Row;
            Match(TokenType.CloseBracket);
            expr = Indexing;
        }
        if(CurrentToken.Type == TokenType.PlusPlus || CurrentToken.Type == TokenType.MinusMinus)
        {
            expr = new UnaryExpressionNode{Operator = CurrentToken.Type, Operand =expr, Atend = true};
            Match(CurrentToken.Type);
        }
        return expr;
    }

    private ExpressionNode ParseBasicPrimaryExpression()
    {
        if (CurrentToken?.Type == TokenType.Minus ||CurrentToken?.Type == TokenType.PlusPlus ||CurrentToken?.Type == TokenType.MinusMinus)
        {
            TokenType op = CurrentToken.Type;
            Match(op);
            ExpressionNode operand = ParsePrimaryExpression();
            operand.row = GetLastToken().Row;
            return new UnaryExpressionNode { Operator = op, Operand = operand, Atend = false};
        }
        if (CurrentToken?.Type == TokenType.Identifier)
        {
            string ID = Match(TokenType.Identifier).Value;
            IdentifierNode Id = new IdentifierNode(ID) ;
            Id.row = GetLastToken().Row;
            if (CurrentToken.Type == TokenType.MinusMinus || CurrentToken.Type == TokenType.PlusPlus)
            {
                Token op =  Match(CurrentToken.Type);
                return new UnaryExpressionNode{Operator = op.Type, Operand = Id, Atend = true};
            }
            else if(CurrentToken.Type == TokenType.SimpleConcat)
            {
                ConcatExpresion expresion = new ConcatExpresion();
                expresion.left = Id;
                Match(TokenType.SimpleConcat);
                expresion.right = ParseExpression();
                expresion.right.row = GetLastToken().Row;
                return expresion;
            }
            else if(CurrentToken.Type == TokenType.CompConcat)
            {
                ConcatExpresion expresion = new ConcatExpresion();
                expresion.left = Id;
                Match(TokenType.CompConcat);
                expresion.right = ParseExpression();
                expresion.right.row = GetLastToken().Row;
                expresion.IsComp = true;
                return expresion;
            }
            return Id;
        }
        else if (CurrentToken?.Type == TokenType.Number)
        {
            return new NumberNode { Value = int.Parse(Match(TokenType.Number).Value) };
        }
        else if (CurrentToken?.Type == TokenType.String)
        {
            StringNode str  = new StringNode { Value = Match(TokenType.String).Value };
            if(CurrentToken.Type == TokenType.SimpleConcat)
            {
                ConcatExpresion expresion = new ConcatExpresion();
                expresion.left = str;
                Match(TokenType.SimpleConcat);
                expresion.right = ParseExpression();
                return expresion;
            }
            else if(CurrentToken.Type == TokenType.CompConcat)
            {
                ConcatExpresion expresion = new ConcatExpresion();
                expresion.row = CurrentToken.Row;
                expresion.left = str;
                Match(TokenType.CompConcat);
                expresion.right = ParseExpression();
                expresion.right.row = GetLastToken().Row;
                expresion.IsComp = true;
                return expresion;
            }
            return str;
        }
        else if (CurrentToken?.Type == TokenType.True || CurrentToken?.Type == TokenType.False)
        {
            BooleanLiteralNode boolean = new BooleanLiteralNode { Value = CurrentToken.Type == TokenType.True };
            boolean.row = CurrentToken.Row;
            Match(CurrentToken.Type);
            return boolean;
        }
        else if (CurrentToken?.Type == TokenType.OpenParen)
        {
            Match(TokenType.OpenParen);
            var expr = ParseExpression();
            expr.row = GetLastToken().Row;
            Match(TokenType.CloseParen);
            return expr;
        }
        else if (CurrentToken?.Type == TokenType.NumberType || CurrentToken?.Type == TokenType.StringType || CurrentToken?.Type == TokenType.BoolType)
        {
            DataTypeNode expresion = new DataTypeNode();
            expresion.type = Match(CurrentToken.Type).Type;
            return expresion;
        } // Para asignacion de propiedades
        else if(CurrentToken?.Type == TokenType.Power || CurrentToken?.Type == TokenType.Name||CurrentToken?.Type == TokenType.Type ||CurrentToken?.Type == TokenType.Range|| CurrentToken?.Type == TokenType.Faction|| CurrentToken?.Type == TokenType.Identifier)
        {
            IdentifierNode prop = new IdentifierNode(Match(CurrentToken.Type).Value);
            prop.row = GetLastToken().Row;
            return prop;
        }
        else
        {
            errorList.Add(new ErrorBlockNode($"Se esperaba una expresión primaria y se recibió: {CurrentToken?.Value} en la linea {CurrentToken?.Row}",new List<Token>{CurrentToken}));
            return null;
        }

    }
    private BlockNode ParseBlock()
    {
        BlockNode block = new BlockNode();
        //Bloque de mas de una instruccion con llave
        if (CurrentToken?.Type == TokenType.OpenBrace)
        {
            Match(TokenType.OpenBrace);
            while (CurrentToken != null && CurrentToken.Type != TokenType.CloseBrace)
            {
                block.Statements.Add(ParseStatement());
                block.Statements.Last().row = GetLastToken().Row;
            }
            Match(TokenType.CloseBrace);
            Match(TokenType.DotCom);                                     
            return block;
        }
        //Bloque de instruccion unica
        while (CurrentToken != null && CurrentToken.Type != TokenType.Comma)
        {
            if(CurrentToken.Type == TokenType.CloseBrace)
            {
                // Match(TokenType.CloseBrace);
                break;
            }
            block.Statements.Add(ParseStatement());
            block.Statements.Last().row = GetLastToken().Row;
        }
        // Match(TokenType.Comma);
        return block;
    }
}

