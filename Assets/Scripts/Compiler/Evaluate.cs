using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameLogic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEditor;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.Rendering;
using System.IO.Compression;
namespace Interpeter
{
    public class Evaluate : MonoBehaviour, IVisitor
    {
        public int ID{get;set;}
        public  Dictionary<int,CardNode> createdCardsNode;
        public List<Card> createdCards;
        public List<string> errors;
        public CompilerManager compiler;
        public Dictionary<string, EffectNode> declaratedEffects; 
        public Evaluate(ProgramNode program,CompilerManager compiler)
        {
            this.compiler = compiler;
            createdCardsNode = new Dictionary<int, CardNode>();
            createdCards = new List<Card>();
            declaratedEffects = new Dictionary<string, EffectNode>();
            ID = -1;
            errors = new List<string>();
            program.Accept(this,null);
        }
        public void Visit(ProgramNode node, Scope scope = null)
        {
            Scope ProgramScope = new Scope();
            foreach (var statement in node.Statements)
            {
                if (statement is EffectNode effect)
                {
                    Debug.Log(effect.Name +  "Nombre del efecto");
                    declaratedEffects[(string)(effect.Name as StringNode).Value] = effect;
                    continue;
                }
                statement.Accept(this,ProgramScope);
            }
        }
        public void Visit(EffectNode node, Scope scope)
        {
            node.Action.Accept(this,scope);
        }

        public void Visit(CardNode node, Scope scope)
        {
            node.Power.Accept(this,scope);
            List<int> cardRows = EvaluateCardRows(node.Range);
            int type = GetCardType((string)(node.Type as StringNode).Value);
            Card CreatedCard = new Card((string)node.Name.Value,(string)node.Faction.Value,"Special Created Card By User",(int)node.Power.Value,ID,null,ExcecuteCreatedCardFunction,cardRows,type);
            Debug.Log(CreatedCard.Faction);
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{CreatedCard.Faction}");
            if (prefab is null)
                return;
            GameObject tagInstance = Instantiate(prefab,compiler.LeftTagsPanel.transform);
            tagInstance.GetComponent<CardCompilerDisplay>().card = CreatedCard;
            tagInstance.GetComponent<CardCompilerDisplay>().CardVisor = compiler.CardVisor;
            tagInstance.GetComponent<CardCompilerDisplay>().cardName.text = CreatedCard.Name;
            createdCardsNode[ID] = node; 
            createdCards.Add(CreatedCard);
            ID --;
        }
        private List<int> EvaluateCardRows(List<string> range)
        {
            List<int> list = new List<int>();
            foreach (string row in range)
            {
                if(row == "Melee")
                    list.Add(1);
                else if(row == "Ranged")
                    list.Add(2);
                else
                    list.Add(3);
            }
            return list;
        } 
        private int GetCardType(string type)
        {
            switch (type)
            {
                case "Oro":
                    return 2;
                case "Plata":
                    return 0;
                case "Clima":
                    return 5;
                case "Aumento":
                    return 6;
                case "Lider":
                    return 3;
            }
            return -1;
        }
        public void Visit(ActivationBlockNode node, Scope scope)
        {
            if(node.effect is not null)
                node.effect.Accept(this,scope);
            else if(node.selector is not null)
                node.selector.Accept(this,scope);
            else if(node.postAction is not null)
                node.postAction.Accept(this,scope);
        }
        public void Visit(EffectBuilderNode node, Scope scope)
        {
            foreach (StatementNode assingnment in node.assingments)
            {
                assingnment.Accept(this,scope);
            }
        }
        public void Visit(SelectorNode node, Scope scope)
        {
            Context playerContext = GameManager.instance.logicGame.PlayerOnTurn().context;
            GameManager.instance.logicGame.PlayerOnTurn().SetContext();
            string sourceName = node.source.Name.Trim('/').Trim('"').Trim().ToString();
            PropertyInfo propertyInfo = playerContext.GetType().GetProperty(sourceName);
            List<Card> source = (List<Card>)propertyInfo.GetValue(playerContext); 
            node.single.Accept(this,scope);
            node.predicate.Accept(this,scope);
            List<Card> targets = new List<Card>();
            foreach (var card in source)
            {
                string propertyName = node.predicate.propertyName.Trim('/').Trim('"').Trim();
                var propertyValue = GetPropertyValue(card,propertyName);
                var filterValue = (node.predicate.filter as BooleanBinaryExpressionNode).Right.Value;
                if (Equals(propertyValue,filterValue))
                {
                    targets.Add(card);
                }
                if((bool)node.single.Value)
                    break;
            }
            node.targets = targets;
        }

        public void Visit(PredicateFunction node, Scope scope)
        {
            (node.filter as BooleanBinaryExpressionNode).Right.Accept(this,scope);
            PropertyAccessNode propertyAccessNode = (node.filter as BooleanBinaryExpressionNode).Left as PropertyAccessNode;
            node.propertyName = propertyAccessNode.PropertyName.Name;
            node.Value = node;
        }
        private object GetPropertyValue(Card card, string propertyName)
        {
            var propertyInfo = card.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                GameManager.instance.errorReporter.ShowError($"La propiedad {propertyName} no existe en el tipo {card.GetType().Name}");
                return null;
            }
            return propertyInfo.GetValue(card);
        }

        public void Visit(ActionBlockNode node, Scope scope)
        {
            foreach(StatementNode statement in node.Statements)
            {
                statement.Accept(this,scope);
            }
        }

        public void Visit(ForStatementNode node, Scope scope)
        {
            List<Card> Collection;
            if(node.Collection is IdentifierNode ID)
            {
                Collection = (List<Card>)scope.Values[ID.Name];
            }
            else
            {
                node.Collection.Accept(this,scope);
                Collection = (List<Card>)node.Collection.Value;
            }
            for (int i = 0; i < Collection.Count; i++)
            {
                //Actualiza el target del foreach de la DSL
                scope.Values[(node.Variable as IdentifierNode).Name] = Collection[i];
                node.Body.Accept(this,scope);
            }
        }

        public void Visit(WhileStatementNode node, Scope scope)
        {
            node.Condition.Accept(this,scope);
            while((bool)node.Condition.Value)
            {
                node.Body.Accept(this,scope);
                node.Condition.Accept(this,scope);
            }
        }

        public void Visit(AssignmentNode node, Scope scope)
        {
            node.Value.Accept(this,scope);
            if(node.Variable is IdentifierNode id)
            {
                scope.AssignVariable(id.Name,node.Value.Value);
            }
            if (node.Variable is PropertyAccessNode property)
            {
                property.Accept(this,scope);
                property.ReferencedObject.SetValue(node.Value.Value);
            }
            if (node.Variable is MethodCallNode methodCall)
            {
                methodCall.Accept(this,scope);
                methodCall.ReferencedObject.SetValue(node.Value.Value);
            }   
        }

        public void Visit(AccesExpressionNode node, Scope scope)
        {
            node.Expression.Accept(this,scope);
        }
        public void Visit(MathBinaryExpressionNode node, Scope scope)
        {
            node.Left.Accept(this,scope);
            node.Right.Accept(this,scope);
            switch (node.Operator)
            {
                case TokenType.Plus:
                    node.Value = (int)node.Left.Value + (int)node.Right.Value;
                    break;
                case TokenType.Minus:
                    node.Value = (int)node.Left.Value - (int)node.Right.Value;
                    break;
                case TokenType.Multiply:
                    node.Value = (int)node.Left.Value * (int)node.Left.Value;
                    break;
                case TokenType.Divide:
                    if((int)node.Left.Value == 0)
                        GameManager.instance.errorReporter.ShowError($"Intento de division por cero en fila {node.Right.row}");
                    node.Value = (int)node.Left.Value / (int)node.Left.Value;
                    break;
            }
        }
        public void Visit(BooleanBinaryExpressionNode node, Scope scope)
        {
            node.Left.Accept(this,scope);
            node.Right.Accept(this,scope);
            bool leftFlag = false;
            bool rightFlag = false;
            if(node.Left is UnaryExpressionNode leftExpression && leftExpression.Atend)
            {
                scope.Values[(leftExpression.Operand as IdentifierNode).Name] = (int)scope.Values[(leftExpression.Operand as IdentifierNode).Name]-1; 
                node.Left.Value = scope.Values[(leftExpression.Operand as IdentifierNode).Name];
                leftFlag = true;
            }
            if(node.Right is UnaryExpressionNode rightExpression && rightExpression.Atend)
            {
                scope.Values[(rightExpression.Operand as IdentifierNode).Name] = (int)scope.Values[(rightExpression.Operand as IdentifierNode).Name]-1; 
                node.Right.Value = scope.Values[(rightExpression.Operand as IdentifierNode).Name];
                rightFlag = true;
            }
            switch (node.Operator)
            {
                case TokenType.And:
                    node.Value = (bool)node.Left.Value && (bool)node.Right.Value;
                    break;
                case TokenType.Or:
                    node.Value = (bool)node.Left.Value || (bool)node.Right.Value;
                    break;
                case TokenType.EqualValue:
                    node.Value = (bool)node.Left.Value == (bool)node.Left.Value;
                    break;
                case TokenType.NotEquals:
                    node.Value = (bool)node.Left.Value != (bool)node.Left.Value;
                    break;
                 case TokenType.LessThan:
                    node.Value = (int)node.Left.Value < (int)node.Right.Value;
                    break;
                case TokenType.GreaterThan:
                    node.Value = (int)node.Left.Value > (int)node.Left.Value;
                    break;
                case TokenType.LessThanOrEquals:
                    node.Value = (int)node.Left.Value <= (int)node.Right.Value;
                    break;
                case TokenType.GreaterThanOrEquals:
                    node.Value = (int)node.Left.Value >= (int)node.Left.Value;
                    break;
            }
            if(leftFlag)
            {
                scope.Values[((node.Left as UnaryExpressionNode).Operand as IdentifierNode).Name] = (int)scope.Values[((node.Left as UnaryExpressionNode).Operand as IdentifierNode).Name]+1; 
                // node.Left.Value = (int)node.Left.Value +1;
            }
            if(rightFlag)
            {
                scope.Values[((node.Right as UnaryExpressionNode).Operand as IdentifierNode).Name] = (int)scope.Values[((node.Right as UnaryExpressionNode).Operand as IdentifierNode).Name]+1;
                // node.Right.Value = (int)node.Left.Value +1; 
            }
        }
        public void Visit(UnaryExpressionNode node, Scope scope){
           
            IdentifierNode operand = node.Operand as IdentifierNode;
            if(node.Operator is TokenType.PlusPlus)
            {
                scope.Values[operand.Name] = (int)scope.Values[operand.Name] + 1;
            }
            else
                scope.Values[operand.Name] = (int)scope.Values[operand.Name] - 1;
        }
        public void Visit(BooleanLiteralNode node, Scope scope){}
        public void Visit(NumberNode node, Scope scope){}
        public void Visit(DataTypeNode node, Scope scope){}
        public void Visit(StringNode node, Scope scope){}
        public void Visit(IdentifierNode node, Scope scope){
            node.Value = scope.Values[node.Name];
        }

        public void Visit(BlockNode node, Scope scope)
        {
            foreach (StatementNode statement in node.Statements)
            {
                statement.Accept(this,scope);
            }
        }

        public void Visit(PropertyAccessNode node, Scope scope)
        {
            object targetObject = null;
            if(node.Target is IdentifierNode id)
            {
                targetObject = scope.GetValue(id.Name);
            }
            else if (node.Target is PropertyAccessNode property)
            {
                property.Accept(this, scope);
                targetObject = property.ReferencedObject;
            }
            else
            {
                node.Target.Accept(this,scope);
                targetObject = node.Target.Value;
            }

            if (targetObject != null)
            {
                Type targetType = targetObject.GetType();
                PropertyInfo propertyInfo;
                //si el objetivo no es una carta entonces la propiedad de analiza en minuscula para casos como .Hand de haga .hand
                //revisar si funciona bien en todos los casos
                if(targetObject is not Card)
                {
                    Debug.Log(node.PropertyName.Name.ToLower());
                    propertyInfo = targetType.GetProperty(node.PropertyName.Name.ToLower()); 
                }                
                else
                {
                    propertyInfo = targetType.GetProperty(node.PropertyName.Name); 
                }

                if (propertyInfo != null)
                {
                    // Guarda la referencia al PropertyInfo y al objeto
                    node.ReferencedObject = new PropertyReference(targetObject, propertyInfo);
                }
                else
                {
                    GameManager.instance.errorReporter.ShowError($"La propiedad '{node.PropertyName.Name}' no se encontró en '{targetType.Name}':fila {node.PropertyName.row}");
                    return;
                }
            }
            else
            {
                GameManager.instance.errorReporter.ShowError($"El objeto {node.Target} es nulo: fila {node.Target.row}");
            }
        }
        public class PropertyReference
        {
            public object TargetObject { get; }
            public PropertyInfo PropertyInfo { get; }
            public PropertyReference(object targetObject, PropertyInfo propertyInfo)
            {
                TargetObject = targetObject;
                PropertyInfo = propertyInfo;
            }
            public object GetValue()
            {
                if(PropertyInfo is null)
                    return TargetObject;
                return PropertyInfo.GetValue(TargetObject);
            }
            public void SetValue(object value)
            {
                PropertyInfo.SetValue(TargetObject, value);
            }
        }

        public void Visit(CollectionIndexingNode node, Scope scope)
        {
            node.Collection.Accept(this,scope);
            node.Index.Accept(this,scope);
            try
            {
                List<Card> collection = (List<Card>)node.Collection.ReferencedObject.GetValue();
                int index = (int)node.Index.Value;
                node.Value = collection[index];
            }
            catch (IndexOutOfRangeException)
            {
                GameManager.instance.errorReporter.ShowError($"Indice fuera de rango : fila {node.Collection.row}");
            }
        }

        public void Visit(CompoundAssignmentNode node, Scope scope)
        {
            node.Value.Accept(this,scope);
            int rightValue = (int)node.Value.Value; 
            TokenType Operator = node.Operator;
            if(node.Variable is IdentifierNode id)
            {
                int leftValue = (int)scope.GetValue(id.Name);
                scope.AssignVariable(id.Name,ExcecuteCompoundExpression(leftValue,rightValue,Operator));
            }
            if (node.Variable is PropertyAccessNode property)
            {
                property.Accept(this,scope);
                int leftValue = (int)property.ReferencedObject.GetValue();
                property.ReferencedObject.SetValue(ExcecuteCompoundExpression(leftValue,rightValue,Operator));
            }
            if (node.Variable is MethodCallNode methodCall)
            {
                methodCall.Accept(this,scope);
                int leftValue = (int)methodCall.ReferencedObject.GetValue();
                methodCall.ReferencedObject.SetValue(ExcecuteCompoundExpression(leftValue,rightValue,Operator));
            }     
        }
        public int ExcecuteCompoundExpression(int leftValue, int rightValue, TokenType Operator)
        {
            switch (Operator)
           {
            case TokenType.PlusEqual:
                return leftValue += rightValue; 
            case TokenType.MinusEqual:
                return leftValue -= rightValue; 
            case TokenType.MultiplyEqual:
                return leftValue *= rightValue; 
            case TokenType.DivideEqual:
                if(rightValue != 0)
                return leftValue /= rightValue;
                break;
           }
           return leftValue;
        }
        public void Visit(MethodCallNode node, Scope scope)
        {
            bool turnPlayer = false;
            string location = "";
            object targetObject = null;
            if(node.Target is PropertyAccessNode property)
            {
                property.Accept(this,scope);
                location = property.PropertyName.Name;
                targetObject = property.ReferencedObject.GetValue();
            }
            if(node.Target is IdentifierNode id)
            {
                targetObject = scope.GetValue(id.Name);
                location = id.Name;
            }
            if(node.Target is MethodCallNode method)
            {
                method.Accept(this,scope);
                targetObject = method.Value;
                location = method.MethodName.Name;
                if(method.Arguments is not null && method.Arguments[0].Value == GameManager.instance.logicGame.PlayerOnTurn())
                    turnPlayer = true;
            }
            if(node.Arguments != null)
                foreach(var argument in node.Arguments)
                {
                    argument.Accept(this,scope);    
                }
            List<Type> argumentTypes = node.Arguments.Select(a => a.Value.GetType()).ToList();
            List<object> argumentValues = node.Arguments.Select(a => a.Value).ToList();

            // logica para adicionar a board o field debido a la flata de especificacion de fila en la DSL
            if (node.MethodName.Name == "Add" || node.MethodName.Name == "SendBottom"  || node.MethodName.Name == "Push")
            {
                GameLogic.Player player = GameManager.instance.logicGame.PlayerOnTurn();
                
                if(location.ToLower() == "board" || location.ToLower() == "field")
                {
                    foreach (int row in (argumentValues[0] as Card).row)
                    {
                        if(row == 1 && player.battleField.contactrow.Count<7)
                        {
                            targetObject = player.battleField.contactrow;
                            break;
                        }
                        if(row == 2 && player.battleField.distantrow.Count <7)
                        {
                            targetObject = player.battleField.distantrow;
                            break;
                        }
                        if(row == 3 && player.battleField.siegerow.Count < 7)
                        {
                             targetObject = player.battleField.siegerow;
                            break;
                        }
                    }
                }
                if(location == "FieldOfPlayer" && !turnPlayer)
                {
                    GameManager.instance.errorReporter.ShowError("Intento agregar una carta al contenido del enemigo y solo puede agregar cartas en las filas propias");
                }
                if((location == "Hand" || location == "hand" || location == "HandOfPlayer") && (targetObject as List<Card>).Count > 10 )
                {
                    GameManager.instance.errorReporter.ShowError("No es posible agregar mas cartas a la mano elegida");
                }
            }
            else if(node.MethodName.Name == "Shuffle")
            {
                if(location == "board" || location == "field"|| location == "FieldofPlayer")
                {
                    GameManager.instance.errorReporter.ShowError("No es posible agregar esa carta a la zona especificada");
                }
            }
            if(targetObject is not null)
            {
                argumentTypes.Add(targetObject.GetType());
                argumentValues.Add(targetObject);
            }
            MethodInfo methodInfo = this.GetType().GetMethod(node.MethodName.Name, argumentTypes.ToArray());
            object result = methodInfo.Invoke(this, argumentValues.ToArray());
            node.Value = result;
        }   
        #region MethodsRegion
        public void Add(Card Card, List<Card> target)
        {
            //Genera una nueva carta con los datos de la original que le dio origen pero con un nuevo ID
            
            Card card = Card.Clone();
            card.ID = GameManager.instance.logicGame.currentID ++;
            if(createdCardsNode.ContainsKey(Card.ID))
            {
                createdCardsNode[card.ID] = createdCardsNode[Card.ID];
            }
            LogicGameManager.CardDictionary[card.ID] = card;
            GameLogic.Player player1 = GameManager.instance.logicGame.player1;
            GameLogic.Player player2 = GameManager.instance.logicGame.player2;
            Player UIplayer1 = GameManager.instance.player1;
            Player UIplayer2 = GameManager.instance.player2;
            if(target == player1.battleField.contactrow)
            {
                player1.totalforce += card.Power;
                card.Owner = player1;
                card.currentRow = 1;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.ActiveCard(card,player1.battleField.contactrow);
                player1.AddCardTo(card, player1.battleField.contactrow);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.attackContainer.transform,UIplayer1.board.attackRow);
            }
            else if(target == player1.battleField.distantrow)
            {
                player1.totalforce += card.Power;
                card.currentRow = 2;
                card.Owner = player1;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.ActiveCard(card,player1.battleField.distantrow);
                player1.AddCardTo(card, player1.battleField.distantrow);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.distantContainer.transform,UIplayer1.board.distantRow);
            }
            else if(target == player1.battleField.siegerow)
            {
                player1.totalforce += card.Power;
                card.currentRow = 3;
                card.Owner = player1;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.ActiveCard(card,player1.battleField.siegerow);
                player1.AddCardTo(card, player1.battleField.siegerow);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.siegeContainer.transform,UIplayer1.board.siegeRow);
            }
            else if(target == player2.battleField.contactrow)
            {
                player2.totalforce += card.Power;
                card.currentRow = 1;
                card.Owner = player2;
                player2.PlayerCardDictionary[card.ID] = card;
                player2.ActiveCard(card,player2.battleField.contactrow);
                player2.AddCardTo(card, player2.battleField.contactrow);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.attackContainer.transform,UIplayer2.board.attackRow);
            }
            else if(target == player2.battleField.distantrow)
            {
                player2.totalforce += card.Power;
                card.currentRow = 2;
                card.Owner = player2;
                player2.PlayerCardDictionary[card.ID] = card;
                player2.ActiveCard(card,player2.battleField.distantrow);
                player2.AddCardTo(card,player2.battleField.distantrow);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.distantContainer.transform,UIplayer2.board.distantRow);
            }
            else if(target == player2.battleField.siegerow)
            {
                player2.totalforce += card.Power;
                card.currentRow = 3;
                card.Owner = player2;
                player2.PlayerCardDictionary[card.ID] = card;
                player2.ActiveCard(card,player2.battleField.siegerow);
                player2.AddCardTo(card, player2.battleField.siegerow);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.siegeContainer.transform,UIplayer2.board.siegeRow);
            }
            else if(target == player1.hand)
            {
                player1.totalforce += card.Power;
                card.currentRow = 0;
                card.Owner = player1;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.hand.Add(card);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.handcontainer.transform,UIplayer1.hand);
            }
            else if( target == player2.hand)
            {
                player2.totalforce += card.Power;
                card.currentRow = 0;
                card.Owner = player2;
                player2.PlayerCardDictionary[card.ID] = card;
                player2.hand.Add(card);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.handcontainer.transform,UIplayer2.hand);
            }
            else if( target == player1.battleField.graveyard)
            {
                player1.totalforce += card.Power;
                card.currentRow = 4;
                card.Owner = player1;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.battleField.graveyard.Add(card);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.graveYardContainer.transform,UIplayer1.board.graveyard);
            }
            else if(target == player2.battleField.graveyard)
            {
                player2.totalforce += card.Power;
                card.currentRow = 4;
                card.Owner = player2;
                player2.PlayerCardDictionary[card.ID] = card;
                player1.battleField.graveyard.Add(card);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.graveYardContainer.transform,UIplayer2.board.graveyard);
            }
            else if(target == player1.deck)
            {
                player1.totalforce += card.Power;
                card.Owner = player1;
                card.currentRow = 5;
                player1.PlayerCardDictionary[card.ID] = card;
                player1.deck.Add(card);
                UIplayer1.AddCardAfterEffect(card.ID,UIplayer1.board.deckcontainer.transform,UIplayer1.CardInstances);
            }
            else if(target == player2.deck)
            {
                player2.totalforce += card.Power;
                card.Owner = player2;
                card.currentRow = 5;
                player2.PlayerCardDictionary[card.ID] = card;
                player2.deck.Add(card);
                UIplayer2.AddCardAfterEffect(card.ID,UIplayer2.board.deckcontainer.transform,UIplayer2.CardInstances);
            }
        }
        public void Push(Card card, List<Card> target)
        {
            target.Add(card);
        }
        public List<Card> HandOfPlayer(GameLogic.Player player)
        {
            return player.context.hand;
        }
        public List<Card> FieldOfPlayer(GameLogic.Player player)
        {
            return player.context.field;
        }
        public List<Card> GraveyardOfPlayer(GameLogic.Player player)
        {
            return player.context.graveyard;
        }
        public List<Card> DeckOfPlayer(GameLogic.Player player)
        {
            return player.context.deck;
        }
        public Card Pop(List<Card> target)
        {
            Card cardret;
            cardret = target[0];
            if(target.Count == 0)
                GameManager.instance.errorReporter.ShowError("No hay cartas en la coleccion indicada");
            cardret.Owner.cardstodelinUI.Add(cardret.ID);
            cardret.Owner.battleField.graveyard.Add(cardret);
            target.RemoveAt(0);
            return cardret;
        }
        public void SendBottom(Card card,List<Card> target)
        {
            this.Add(card,target);
        }
        public void Remove(Card card,List<Card> target)
        {
            card.Owner.cardstodelinUI.Add(card.ID);
            card.Owner.battleField.graveyard.Add(card);                 
            target.Remove(card);
        }
        public List<Card> Find(PredicateFunction predicate, List<Card> target)
        {
            List<Card> filter = new List<Card>();
            foreach (var card in target)
            {
                var propertyValue = GetPropertyValue(card,predicate.propertyName);
                if (Equals(propertyValue,(predicate.filter as BooleanBinaryExpressionNode).Right.Value))
                {
                    filter.Add(card);
                }
            }
            return filter;
        }
        public void Shuffle(List<Card> target)
        {
            DeckManager.Shuffle(target);
            List<int> cardsOrder = target.Select(x => x.ID).ToList();
            if(target.Count != 0 && target[0].currentRow == 0)
            {
                if(target[0].currentRow == 0)
                {
                    if(target[0].Owner == GameManager.instance.logicGame.player1)
                    {
                        GameManager.instance.player1.ShuffleCards(GameManager.instance.player1.hand,GameManager.instance.player1.board.handcontainer,cardsOrder);
                    }
                    else if(target[0].Owner == GameManager.instance.logicGame.player2)
                    {
                        GameManager.instance.player2.ShuffleCards(GameManager.instance.player2.hand,GameManager.instance.player2.board.handcontainer,cardsOrder);
                    }
                }
                if(target[0].currentRow == 4)
                {
                    if(target[0].Owner == GameManager.instance.logicGame.player1)
                    {
                        GameManager.instance.player1.ShuffleCards(GameManager.instance.player1.graveyard,GameManager.instance.player1.board.graveYardContainer,cardsOrder);
                    }
                    else if(target[0].Owner == GameManager.instance.logicGame.player2)
                    {
                        GameManager.instance.player2.ShuffleCards(GameManager.instance.player2.graveyard,GameManager.instance.player2.board.graveYardContainer,cardsOrder);
                    }
                }
                if(target[0].currentRow == 5)
                {
                    if(target[0].Owner == GameManager.instance.logicGame.player1)
                    {
                        GameManager.instance.player1.ShuffleCards(GameManager.instance.player1.CardInstances,GameManager.instance.player1.board.deckcontainer,cardsOrder);
                    }
                    else if(target[0].Owner == GameManager.instance.logicGame.player2)
                    {
                        GameManager.instance.player2.ShuffleCards(GameManager.instance.player2.CardInstances,GameManager.instance.player2.board.deckcontainer,cardsOrder);
                    }
                }
            }
        }
        #endregion
        public void Visit(ConcatExpresion node, Scope scope)
        {
            SemanticChecker.EvaluateLiteralExpresionNode(node);
        }
        public void ExcecuteCreatedCardFunction(Card card,List<Card>row)
        {
            CardNode createdCard =  createdCardsNode[card.ID];
            GameManager.instance.errorReporter.ShowError($"Efecto ejecutado, aqui se mostraran los errores en caso de existir");
            foreach(ActivationBlockNode effectcall in createdCard.OnActivationBlock)
            {
                ExcecuteCreatedCardFunction(effectcall);
            }
        }
        public void ExcecuteCreatedCardFunction(ActivationBlockNode effectCall)
        {
            
            EffectNode referencedEffect = declaratedEffects[(string)effectCall.effect.Name.Value];
            Scope EffectScope = new Scope();
            for (int i = 0; i < referencedEffect.Params.Count; i++)
            {
                //Asigna los valores reales del juego a cada parametro de efecto
                //effectCall.effect.assingments[i].Accept(this,null);
                EffectScope.Values[((effectCall.effect.assingments[i] as AssignmentNode).Variable as IdentifierNode).Name] = (effectCall.effect.assingments[i] as AssignmentNode).Value.Value;
            }
            effectCall.selector.Accept(this,null);
            EffectScope.Values[(referencedEffect.Action.target.Variable as IdentifierNode).Name] = effectCall.selector.targets;
            EffectScope.Values[(referencedEffect.Action.context.Variable as IdentifierNode).Name] =  GameManager.instance.logicGame.PlayerOnTurn().context;
            EffectScope.Values["TriggerPlayer"] = GameManager.instance.logicGame.PlayerOnTurn();
            //ejecuta el efecto con los parametros ya recibidos
            referencedEffect.Accept(this,EffectScope);
            if(effectCall.postAction is not null)
                ExcecuteCreatedCardFunction(effectCall.postAction);
        }
    }
}
