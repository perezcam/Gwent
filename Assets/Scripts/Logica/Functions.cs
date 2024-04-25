using System.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Buffers;
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
                
                if(Card!=null && Card.type!=2)
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
            card.owner.battleField.SetWeather(row,1);
            enemy.battleField.SetWeather(enemyrow,1);
            card.owner.battleField.SetWeather(row,1);
            
            foreach (Card Card in row)
            {
                if(Card!=null && Card.powerattack<4 && Card.type!=2)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = 0;
                }
            }

            foreach (Card Card in enemyrow)
            {
               if(Card!=null && Card.powerattack<=4 && Card.type!=2)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = 0;
                }
            }
            card.owner.totalforce += card.powerattack;
            
            if(card.powerattack!=0)
                card.owner.W_cardstoshowinUI.Add(card.ID);    
        }
        
        //Clima tipo 2:Elimina todas las cartas de tipo Increase y el incremento provocado por ellas y ademas si usa otro Increase mientas ella este activa lo elimina
        public static void W_ResetCardValues(Card card,List<Card> row)
        {
            card.owner.totalforce += card.powerattack;
            Player enemy = card.owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row);
            card.owner.battleField.SetWeather(row,2);
            enemy.battleField.SetWeather(enemyrow,2);

            List<Card> cardtoremove = new List<Card>();

            foreach (Card Card in row)
            {
                if (Card != null && Card.type!=2)
                {
                     Card.owner.totalforce -= Card.powerattack - Card.initialPowerAttack;
                     Card.powerattack = Card.initialPowerAttack;
                      //encuentra todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        card.owner.cardstodelinUI.Add(Card.ID);
                        cardtoremove.Add(card);
                     }
                }
            }  
            //elimina todas las cartas con poder IncreasePowerRow en la fila
            foreach (Card removeCard in cardtoremove)
            {
               row.Remove(removeCard);
            }

            cardtoremove = new List<Card>();

             foreach (Card Card in enemyrow)
            {
                if (Card != null && Card.type!=2)
                {
                     Card.owner.totalforce -= (Card.powerattack - Card.initialPowerAttack);
                     Card.powerattack = Card.initialPowerAttack;
                     //encuentra todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        enemy.cardstodelinUI.Add(Card.ID);
                        enemy.totalforce -= Card.powerattack;
                        cardtoremove.Add(Card);
                     }
                }
            }   
            //elimina todas las cartas con poder IncreasePowerRow en la fila
            foreach (Card removeCard in cardtoremove)
            {
                enemyrow.Remove(removeCard);
            }

            if(card.powerattack!=0)
                card.owner.W_cardstoshowinUI.Add(card.ID);
        }    

        // Despeje: Remueve una carta de tipo clima que este afectando a una fila donde es colocada
        public static void RemoveWeatherCard(Card card, List<Card> row)
        {
            card.owner.totalforce += card.powerattack;
            Player enemy = card.owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row);
            Card weatherCard = null;

            foreach (Card Card in row)
            {   //Identifica la carta clima en la fila
                if(Card != null && (Card.owner.battleField.GetWeather(row) == 1 || Card.owner.battleField.GetWeather(row)==2) && Card.type!=2 && (Card.cardfunction.function==W_ReducePowerOfWeakCards || Card.cardfunction.function == W_ResetCardValues))
                {
                    card.owner.battleField.SetWeather(row,0);
                    card.owner.cardstodelinUI.Add(Card.ID); 
                    RemuveSpecialCardsEfects(Card,row);
                    weatherCard = Card;
                }
            }
            //elimina la carta clima
            if (weatherCard != null)
                row.Remove(weatherCard);
            //elimina la referencia anterior
            weatherCard = null;

            foreach (Card Card in enemyrow)
            {   //Identifica la carta clima en la fila de enemigo y la elimina
                if(Card != null && (enemy.battleField.GetWeather(enemyrow) == 1 || enemy.battleField.GetWeather(enemyrow)==2) && Card.type!=2 && (Card.cardfunction.function==W_ReducePowerOfWeakCards || Card.cardfunction.function == W_ResetCardValues))
                {
                    enemy.battleField.SetWeather(enemyrow,0);
                    Card.owner.cardstodelinUI.Add(Card.ID);
                    RemuveSpecialCardsEfects(Card,enemyrow);
                    weatherCard = Card;
                }
            }
            //elimina la carta clima
            if (weatherCard != null)
                row.Remove(weatherCard);
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
            Card powerfullestCard = FindCardByPower(enemyBattleField,1);
            if (powerfullestCard is null || powerfullestCard.type == 2)
                return;
            
            enemy.cardstodelinUI.Add(powerfullestCard.ID);
            enemy.totalforce -= powerfullestCard.powerattack;
            
            if(enemy.battleField.contactrow.Contains(powerfullestCard))
            {
                 RemuveSpecialCardsEfects(powerfullestCard,enemy.battleField.contactrow);
                 enemy.battleField.contactrow.Remove(powerfullestCard);
            }
            else if(enemy.battleField.distantrow.Contains(powerfullestCard))
            {
                 RemuveSpecialCardsEfects(powerfullestCard,enemy.battleField.distantrow);
                 enemy.battleField.distantrow.Remove(powerfullestCard);
            }
            else 
            {
                 RemuveSpecialCardsEfects(powerfullestCard,enemy.battleField.distantrow);
                 enemy.battleField.siegerow.Remove(powerfullestCard);
            }
        }  
        //Elimina la carta con menos poder del campo enemigo
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
            Card weakestcard = FindCardByPower(enemyBattleField,0);
            if (weakestcard is null || weakestcard.type == 2)
                return;
           
            enemy.cardstodelinUI.Add(weakestcard.ID);
            enemy.totalforce -= weakestcard.powerattack;

            if(enemy.battleField.contactrow.Contains(weakestcard))
            {
                 RemuveSpecialCardsEfects(weakestcard,enemy.battleField.contactrow);
                 enemy.battleField.contactrow.Remove(weakestcard);
            }
            else if(enemy.battleField.distantrow.Contains(weakestcard))
            {
                 RemuveSpecialCardsEfects(weakestcard,enemy.battleField.distantrow);
                 enemy.battleField.distantrow.Remove(weakestcard);
            }
            else 
            {
                 RemuveSpecialCardsEfects(weakestcard,enemy.battleField.distantrow);
                 enemy.battleField.siegerow.Remove(weakestcard);
            }
        }
        //Roba una carta de mazo
        public static void StealCard(Card card,List<Card> row)
        {
            card.owner.totalforce+=card.powerattack;
            card.owner.cardstodelinUI.Add(2024);
        }

        //Multiplica por n el ataque de la carta siendo n la cantidad de cartas iguales a ella en el campo
        public static void AttackMultiplier(Card card,List<Card> row)
        {
            Player enemy = card.owner.GetEnemy();
            List<Card> Battlefield = new List<Card>();
            int number = 1;
            Battlefield.AddRange(card.owner.battleField.contactrow);
            Battlefield.AddRange(card.owner.battleField.distantrow);
            Battlefield.AddRange(card.owner.battleField.siegerow);
            Battlefield.AddRange(enemy.battleField.contactrow);
            Battlefield.AddRange(enemy.battleField.distantrow);
            Battlefield.AddRange(enemy.battleField.siegerow);
            
            foreach (Card Card in Battlefield)
            {
                if (Card.name==card.name)
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
            List<Card>[]  Rows = {enemy.contactrow, enemy.distantrow, enemy.siegerow,
                                  owner.contactrow,owner.distantrow,owner.siegerow};
            List<Card> targetrow = new List<Card>();
            int index = -1;
            int min = 10;
            for (int i = 0; i < Rows.Length; i++)
            {
                if(Rows[i].Count()!=0 && Rows[i].Count()<min)
                {
                    index = i;
                    min = Rows[i].Count();
                }
            }
            if(index!=-1)
                targetrow = Rows[index];
            else
                return;     

            foreach(Card Card in targetrow)
            {
                if(Card.type == 1 || Card.type == 2)
                    continue;
                RemuveSpecialCardsEfects(Card,targetrow);
                 targetrow.ElementAt(0).owner.totalforce-= Card.powerattack;
                targetrow.ElementAt(0).owner.cardstodelinUI.Add(Card.ID);
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
            avaragepower = sum/(ownBattefield.Count()+1);

            foreach (Card Card in card.owner.battleField.contactrow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = avaragepower;
                    Card.owner.totalforce += Card.powerattack;
                }
            }
             foreach (Card Card in card.owner.battleField.distantrow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = avaragepower;
                    Card.owner.totalforce += Card.powerattack;
                }
            }
             foreach (Card Card in card.owner.battleField.siegerow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.owner.totalforce -= Card.powerattack;
                    Card.powerattack = avaragepower;
                    Card.owner.totalforce += Card.powerattack;
                }
            }
            card.powerattack = avaragepower;
            card.owner.totalforce+= card.powerattack;
        }

        //Carta senuelo: permite cambiar una carta de valor 0 por otra que este sobre la mesa desactivando los efectos ocasionados por la misma al existir
        public static void Decoy(Card card , List<Card> row)
        {
            RemuveSpecialCardsEfects(card,row);
        }

        // Recursivamente busca la carta con mayor(value=1) o menor(value=0) poder de ataque que no sea de incremento o clima
        private static Card FindCardByPower(List<Card> enemyBattleField, int value)
        {  
            //Caso base
            if(enemyBattleField.Count()==0)
            {
                return null;
            }
            Card card;
            if(value == 1)
                card = enemyBattleField.OrderByDescending((card)=> card.powerattack).ToList().ElementAt(0); 
            else
                card = enemyBattleField.OrderBy((card)=> card.powerattack).ToList().ElementAt(0); 
            
            if (card.type==1)
            {
                enemyBattleField.Remove(card);
                return FindCardByPower(enemyBattleField,value);
            }
            return card;
        }

        public static void RemuveSpecialCardsEfects (Card card, List<Card> row)
        {
            if (card.cardfunction.function == IncreasePowerRow||card.cardfunction.function == W_ReducePowerOfWeakCards||card.cardfunction.function == W_ResetCardValues)
            {
                if(card.cardfunction.function == IncreasePowerRow)
                {
                    DeleteIncrease(card,row);
                }
                else if(card.cardfunction.function == W_ReducePowerOfWeakCards)
                {
                    BeforeW_ReducePowerOfWeakCards(card,row);
                    card.owner.battleField.SetWeather(card.owner.GetRow(card.row),0);
                }
                else
                {
                    card.owner.battleField.SetWeather(card.owner.GetRow(card.row),0);
                }
            }
            else
                return;  
        }        
        private static void DeleteIncrease (Card card, List<Card> row)
        {
            foreach (Card Card in row)
            {
                if ((Card.type != 1 || Card.cardfunction.function == IncreasePowerRow) && card.ID != Card.ID && Card.type!=2) 
                {
                    Card.owner.totalforce -= card.powerattack;
                    Card.powerattack -= card.powerattack;
                }
            }
        }
        private static void BeforeW_ReducePowerOfWeakCards(Card card, List<Card> row)
        {   //Si la carta tiene poder original menor que 4 le incrementa al poder actual de la carta su poder actual
            foreach (Card Card in row)
            {
                if(card.ID != Card.ID && Card.initialPowerAttack<=4 && Card.type!=2)
                {
                    Card.powerattack += Card.initialPowerAttack;
                    Card.owner.totalforce += Card.initialPowerAttack; 
                }    
            }
        }
    }
}