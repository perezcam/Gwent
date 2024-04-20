using System.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
namespace GameLogic
{
    public delegate void SpecialFunction(Card card,List<Card>row); 
    public class Functions
    {
        public Functions()
        {
            
        }
        public SpecialFunction function;
        
        //Si la carta no tiene una funcion especifica solamente le incrementa al jugador sus puntos
          public static void NullFunction (Card card,List<Card>cardsrow)
        {
            card.owner.totalforce += card.powerattack;
        }
        
        // // Aumenta el poder de las cartas de la fila en 2 veces su poder si ya existia otra carta de su tipo
        // //Si no existia incrementa el poder de cada carta en el suyo    
        public static void IncreasePowerRow(Card card,List<Card>row) 
        {
            int num=0;
            int incrFactor = 2;
            // Modificar el factor de incremento mde acuerdo al porciento de incremento deseado
            // cambie esto si se rompre volver a  aanalizar aqui
            foreach (Card Card in row)
            {
                num =(card.powerattack*incrFactor)-card.powerattack;
                
                if(Card!=null)
                {
                    Card.powerattack += num;
                    Card.owner.totalforce+=num;
                }
            }
            card.owner.totalforce+= card.powerattack;
        }
        //Funcion clima 1: todas las cartas con poder menor o igual que 4 baja a 0;
        public static void W_ReducePowerOfWeakCards(Card card, List<Card>row)
        {
            Player enemy = card.owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row);
            card.On_W_ReducePowerOfWeakCards = true;
            foreach (Card Card in row)
            {
                Card.On_W_ReducePowerOfWeakCards = true;
                if(Card!=null && Card.powerattack<4)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = 0;
                }
            }

            foreach (Card Card in enemyrow)
            {
                Card.On_W_ReducePowerOfWeakCards = true;
                Card.owner.totalforce -= Card.powerattack;
                Card.powerattack = 0;
            }
            card.owner.totalforce += card.powerattack;
            
            if(card.powerattack!=0)
                card.owner.W_cardstoshowinUI.Add(card.ID);    
        }
        
        // Clima 2 Restablece los valores de poder de las cartas hasta antes de realizar una un IncreasePowerRow
        //Ademas no permite usar otro Increase en esa fila mientras ella este activa
        public static void W_ResetCardValues(Card card,List<Card> row)
        {
            Player enemy = card.owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row);
            card.On_W_ResetCardValues = true;

            foreach (Card Card in row)
            {
                Card.On_W_ResetCardValues = true;
                if (Card != null)
                {
                     Card.owner.totalforce -= Card.powerattack - Card.initialPowerAttack;
                     Card.powerattack = Card.initialPowerAttack;
                     //elimina todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        card.owner.cardstodelinUI.Add(Card.ID);
                        row.Remove(card);
                     }
                }
            }    
             foreach (Card Card in enemyrow)
            {
                Card.On_W_ResetCardValues = true;
                if (Card != null)
                {
                     Card.owner.totalforce -= (Card.powerattack - Card.initialPowerAttack);
                     Card.powerattack = Card.initialPowerAttack;
                     //elimina todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        enemy.cardstodelinUI.Add(Card.ID);
                        enemyrow.Remove(Card);
                     }
                }
            }   
            if(card.powerattack!=0)
                card.owner.W_cardstoshowinUI.Add(card.ID);
        }    

        // Despeje: Remueve una carta de tipo clima que este afectando a una fila donde es colocada
        public static void RemoveWeatherCard(Card card, List<Card> row)
        {
            Player enemy = card.owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row);

            foreach (Card Card in row)
            {   //Identifica la carta clima en la fila y la elimina
                if(Card != null && row.First().On_W_ReducePowerOfWeakCards)
                {
                    Card.On_W_ReducePowerOfWeakCards = false;
                    if(Card.cardfunction.function == W_ReducePowerOfWeakCards)
                    {
                        card.owner.cardstodelinUI.Add(Card.ID); 
                        row.Remove(Card);
                    }
                }
                else if(Card!=null &&  row.First().On_W_ReducePowerOfWeakCards )
                {
                    Card.On_W_ResetCardValues = false;
                    if(Card.cardfunction.function == W_ResetCardValues)
                    {
                        card.owner.cardstodelinUI.Add(Card.ID); 
                        row.Remove(Card);
                    }
                }
            }

            foreach (Card Card in enemyrow)
            {   //Identifica la carta clima en la fila y la elimina
                if(Card != null && enemyrow.First().On_W_ReducePowerOfWeakCards)
                {
                    Card.On_W_ReducePowerOfWeakCards = false;
                }
                else if(Card!=null &&  enemyrow.First().On_W_ReducePowerOfWeakCards )
                {
                    Card.On_W_ResetCardValues = false;
                }
            }
        }

        // Elimina la carta con mas poder del enemigo
        public static void DelPowerfullCard (Card card,List<Card> row)
        {    
            card.owner.totalforce += card.powerattack;
            Player enemy = card.owner.GetEnemy();            
            List<Card> enemyBattleField = new List<Card>();
            
            enemyBattleField.AddRange(enemy.battleField.contactrow);
            enemyBattleField.AddRange(enemy.battleField.distantrow);
            enemyBattleField.AddRange(enemy.battleField.siegerow);
            if(enemyBattleField.Count()==0)
            {
                return;
            }
            Card powerfullestCard = enemyBattleField.OrderByDescending((card)=> card.powerattack).ToList().ElementAt(0); 
            enemy.cardstodelinUI.Add(powerfullestCard.ID);
            enemy.totalforce -= powerfullestCard.powerattack;
            
            if(enemy.battleField.contactrow.Contains(powerfullestCard))
                 enemy.battleField.contactrow.Remove(powerfullestCard);
            else if(enemy.battleField.distantrow.Contains(powerfullestCard))
                 enemy.battleField.distantrow.Remove(powerfullestCard);
            else 
                enemy.battleField.siegerow.Remove(powerfullestCard);
        }  
        //Elimina la carta con menos poder del campo propio
        public static void DelWeakestCard (Card card,List<Card> row)
        {    
            card.owner.totalforce += card.powerattack;
            Player enemy = card.owner.GetEnemy();            
            List<Card> enemyBattleField = new List<Card>();
            
            enemyBattleField.AddRange(enemy.battleField.contactrow);
            enemyBattleField.AddRange(enemy.battleField.distantrow);
            enemyBattleField.AddRange(enemy.battleField.siegerow);

            if(enemyBattleField.Count()==0)
            {
                return;
            }
            Card powerfullestCard = enemyBattleField.OrderBy((card)=> card.powerattack).ToList().ElementAt(0); 
            enemy.cardstodelinUI.Add(powerfullestCard.ID);
            enemy.totalforce -= powerfullestCard.powerattack;

            if(enemy.battleField.contactrow.Contains(powerfullestCard))
                 enemy.battleField.contactrow.Remove(powerfullestCard);
            else if(enemy.battleField.distantrow.Contains(powerfullestCard))
                 enemy.battleField.distantrow.Remove(powerfullestCard);
            else 
                enemy.battleField.siegerow.Remove(powerfullestCard);
        }
        //Roba una carta de mazo
        public static void StealCard(Card card,List<Card> row)
        {
            card.owner.totalforce+=card.powerattack;
            card.owner.cardstodelinUI.Add(2024);
        }

        //Multiplica por n el ataque de la carta siendo n la cantidad de cartas de tu tipo en el campo
        public static void AttackMultiplier(Card card,List<Card> row)
        {
            Player enemy = card.owner.GetEnemy();
            List<Card> Battlefield = new List<Card>();
            int number = 0;
            Battlefield.AddRange(card.owner.battleField.contactrow);
            Battlefield.AddRange(card.owner.battleField.distantrow);
            Battlefield.AddRange(card.owner.battleField.siegerow);
            Battlefield.AddRange(enemy.battleField.contactrow);
            Battlefield.AddRange(enemy.battleField.distantrow);
            Battlefield.AddRange(enemy.battleField.siegerow);
            
            foreach (Card Card in Battlefield)
            {
                if (Card.ID==card.ID)
                {
                   number++; 
                }
            }
            card.powerattack = number*card.powerattack;
            card.owner.totalforce += card.powerattack;
        }

        //Limpia la fila del campo (no vacia) con menos unidades
        public static void CleanRow(Card card,List<Card> row)
        {
            card.owner.totalforce += card.powerattack;
            BattleField owner = card.owner.battleField;
            BattleField enemy = card.owner.GetEnemy().battleField;
            List<Card>[]  Rows = {owner.contactrow,owner.distantrow,owner.siegerow,
                                  enemy.contactrow, enemy.distantrow, enemy.siegerow};
            List<Card> targetrow = new List<Card>();
            
            int index = 0;
            //Busca la fila de mayor tamano y guarda su indice para saber el jugador que la contiene
            for (int i = 0; i < Rows.Length; i++)
            {
                if(Rows[i].Count()!=0 && Rows[i].Count()<targetrow.Count())
                {
                    targetrow = Rows[i];
                    index = i;
                }

            }
            foreach(Card Card in targetrow)
            {
                if (index>2)
                {
                    card.owner.GetEnemy().cardstodelinUI.Add(Card.ID);
                    card.owner.GetEnemy().totalforce -= Card.powerattack; 
                }
                else
                {
                   card.owner.cardstodelinUI.Add(Card.ID);
                   card.owner.totalforce -= Card.powerattack;
                }

            }
        }

        //Calcula el promedio de poder entre todas las cartas del campo propio .Luego iguala
        //el poder de todas las cartas del campo propio  a ese promedio
        public static void AvaragedPower(Card card,List<Card> row)
        {
            List<Card> ownBattefield = new List<Card>();
            ownBattefield.AddRange(card.owner.battleField.contactrow);
            ownBattefield.AddRange(card.owner.battleField.distantrow);
            ownBattefield.AddRange(card.owner.battleField.siegerow);
            int sum = card.powerattack;
            int avaragepower=0;
           
            foreach(Card Card in ownBattefield)
            {
                sum += Card.powerattack; 
            }
            if(ownBattefield.Count()==0)
            {
                card.owner.totalforce = card.powerattack;
                return;
            }
            avaragepower = sum/ownBattefield.Count()+1;

            foreach (Card Card in card.owner.battleField.contactrow)
            {
                Card.owner.totalforce -= Card.powerattack;
                Card.powerattack = avaragepower;
                Card.owner.totalforce += Card.powerattack;
            }
             foreach (Card Card in card.owner.battleField.distantrow)
            {
                Card.owner.totalforce -= Card.powerattack;
                Card.powerattack = avaragepower;
                Card.owner.totalforce += Card.powerattack;
            }
             foreach (Card Card in card.owner.battleField.siegerow)
            {
                Card.owner.totalforce -= Card.powerattack;
                Card.powerattack = avaragepower;
                Card.owner.totalforce += Card.powerattack;
            }
            card.powerattack = avaragepower;
            card.owner.totalforce+= card.powerattack;
        }
    }
}