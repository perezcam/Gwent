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
            card.Owner.totalforce += card.Power;
        }
        
        // // Aumenta el poder de las cartas de la fila en 2 veces su poder si ya existia otra carta de su tipo
        // //Si no existia incrementa el poder de cada carta en el suyo    
        public static void IncreasePowerRow(Card card,List<Card>row) 
        {
            int num=0;
            int incrFactor = 2;
          
            foreach (Card Card in row)
            {
                num =(card.Power*incrFactor)-card.Power;
                
                if(Card!=null && Card.type!=2)
                {
                    Card.Power += num;
                    Card.Owner.totalforce+=num;
                }
            }
            card.Owner.totalforce+= card.Power;
        }
        //Funcion clima 1: todas las cartas con poder menor o igual que 4 baja a 0;
        public static void W_ReducePowerOfWeakCards(Card card, List<Card>row)
        {
            Player enemy = card.Owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row.First());
            card.Owner.battleField.SetWeather(row,1);
            enemy.battleField.SetWeather(enemyrow,1);
            card.Owner.battleField.SetWeather(row,1);
            
            foreach (Card Card in row)
            {
                if(Card!=null && Card.Power<4 && Card.type!=2)
                {
                    Card.Owner.totalforce -= Card.Power;
                    Card.Power = 0;
                }
            }

            foreach (Card Card in enemyrow)
            {
               if(Card!=null && Card.Power<=4 && Card.type!=2)
                {
                    Card.Owner.totalforce -= Card.Power;
                    Card.Power = 0;
                }
            }
            card.Owner.totalforce += card.Power;
            
            if(card.Power!=0)
                card.Owner.W_cardstoshowinUI.Add(card.ID);    
        }
        
        //Clima tipo 2:Elimina todas las cartas de tipo Increase y el incremento provocado por ellas y ademas si usa otro Increase mientas ella este activa lo elimina
        public static void W_ResetCardValues(Card card,List<Card> row)
        {
            card.Owner.totalforce += card.Power;
            Player enemy = card.Owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row.First());
            card.Owner.battleField.SetWeather(row,2);
            enemy.battleField.SetWeather(enemyrow,2);

            List<Card> cardtoremove = new List<Card>();

            foreach (Card Card in row)
            {
                if (Card != null && Card.type!=2)
                {
                     Card.Owner.totalforce -= Card.Power - Card.initialPowerAttack;
                     Card.Power = Card.initialPowerAttack;
                      //encuentra todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        card.Owner.cardstodelinUI.Add(Card.ID);
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
                     Card.Owner.totalforce -= (Card.Power - Card.initialPowerAttack);
                     Card.Power = Card.initialPowerAttack;
                     //encuentra todas las cartas con poder IncreasePowerRow en la fila
                     if(Card.cardfunction.function == IncreasePowerRow)
                     {
                        enemy.cardstodelinUI.Add(Card.ID);
                        enemy.totalforce -= Card.Power;
                        cardtoremove.Add(Card);
                     }
                }
            }   
            //elimina todas las cartas con poder IncreasePowerRow en la fila
            foreach (Card removeCard in cardtoremove)
            {
                enemyrow.Remove(removeCard);
            }

            if(card.Power!=0)
                card.Owner.W_cardstoshowinUI.Add(card.ID);
        }    

        // Despeje: Remueve una carta de tipo clima que este afectando a una fila donde es colocada
        public static void RemoveWeatherCard(Card card, List<Card> row)
        {
            card.Owner.totalforce += card.Power;
            Player enemy = card.Owner.GetEnemy();
            List<Card> enemyrow = enemy.GetRow(card.row.First());
            Card weatherCard = null;

            foreach (Card Card in row)
            {   //Identifica la carta clima en la fila
                if(Card != null && (Card.Owner.battleField.GetWeather(row) == 1 || Card.Owner.battleField.GetWeather(row)==2) && Card.type!=2 && (Card.cardfunction.function==W_ReducePowerOfWeakCards || Card.cardfunction.function == W_ResetCardValues))
                {
                    card.Owner.battleField.SetWeather(row,0);
                    card.Owner.cardstodelinUI.Add(Card.ID); 
                    RemoveEspecialCardEffect(Card,row);
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
                    Card.Owner.cardstodelinUI.Add(Card.ID);
                    RemoveEspecialCardEffect(Card,enemyrow);
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
            card.Owner.totalforce += card.Power;
            Player enemy = card.Owner.GetEnemy();            
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
            enemy.totalforce -= powerfullestCard.Power;
            
            if(enemy.battleField.contactrow.Contains(powerfullestCard))
            {
                 RemoveEspecialCardEffect(powerfullestCard,enemy.battleField.contactrow);
                 enemy.battleField.contactrow.Remove(powerfullestCard);
            }
            else if(enemy.battleField.distantrow.Contains(powerfullestCard))
            {
                 RemoveEspecialCardEffect(powerfullestCard,enemy.battleField.distantrow);
                 enemy.battleField.distantrow.Remove(powerfullestCard);
            }
            else 
            {
                 RemoveEspecialCardEffect(powerfullestCard,enemy.battleField.distantrow);
                 enemy.battleField.siegerow.Remove(powerfullestCard);
            }
        }  
        //Elimina la carta con menos poder del campo enemigo
        public static void DelWeakestCard (Card card,List<Card> row)
        {    
            card.Owner.totalforce += card.Power;
            Player enemy = card.Owner.GetEnemy();            
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
            enemy.totalforce -= weakestcard.Power;

            if(enemy.battleField.contactrow.Contains(weakestcard))
            {
                 RemoveEspecialCardEffect(weakestcard,enemy.battleField.contactrow);
                 enemy.battleField.contactrow.Remove(weakestcard);
            }
            else if(enemy.battleField.distantrow.Contains(weakestcard))
            {
                 RemoveEspecialCardEffect(weakestcard,enemy.battleField.distantrow);
                 enemy.battleField.distantrow.Remove(weakestcard);
            }
            else 
            {
                 RemoveEspecialCardEffect(weakestcard,enemy.battleField.distantrow);
                 enemy.battleField.siegerow.Remove(weakestcard);
            }
        }
        //Roba una carta de mazo
        public static void StealCard(Card card,List<Card> row)
        {
            card.Owner.totalforce+=card.Power;
            card.Owner.cardstodelinUI.Add(2024);
        }

        //Multiplica por n el ataque de la carta siendo n la cantidad de cartas iguales a ella en el campo
        public static void AttackMultiplier(Card card,List<Card> row)
        {
            Player enemy = card.Owner.GetEnemy();
            List<Card> Battlefield = new List<Card>();
            int number = 1;
            Battlefield.AddRange(card.Owner.battleField.contactrow);
            Battlefield.AddRange(card.Owner.battleField.distantrow);
            Battlefield.AddRange(card.Owner.battleField.siegerow);
            Battlefield.AddRange(enemy.battleField.contactrow);
            Battlefield.AddRange(enemy.battleField.distantrow);
            Battlefield.AddRange(enemy.battleField.siegerow);
            
            foreach (Card Card in Battlefield)
            {
                if (Card.Name==card.Name)
                {
                   number++; 
                }
            }
            card.Power = number*card.Power;
            card.Owner.totalforce += card.Power;
        }

        //Limpia la fila del campo (no vacia) con menos unidades
        public static void CleanRow(Card card,List<Card> row)
        {
            card.Owner.totalforce += card.Power;
            BattleField Owner = card.Owner.battleField;
            BattleField enemy = card.Owner.GetEnemy().battleField;
            List<Card>[]  Rows = {enemy.contactrow, enemy.distantrow, enemy.siegerow,
                                  Owner.contactrow,Owner.distantrow,Owner.siegerow};
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

            for (int i = 0; i < targetrow.Count(); i++)
            
            {
                if(targetrow.ElementAt(i).type == 1 || targetrow.ElementAt(i).type == 2)
                    continue;
                RemoveEspecialCardEffect(targetrow.ElementAt(i),targetrow);
                targetrow.ElementAt(0).Owner.totalforce-= targetrow.ElementAt(i).Power;
                targetrow.ElementAt(0).Owner.cardstodelinUI.Add(targetrow.ElementAt(i).ID);
                //Eliminar carta de la Logica
                targetrow.ElementAt(0).Owner.GetRow(targetrow.ElementAt(0).row.First()).Remove(targetrow.ElementAt(i));
            }
        }

        //Calcula el promedio de poder entre todas las cartas del campo propio .Luego iguala
        //el poder de todas las cartas del campo propio  a ese promedio
        public static void AvaragedPower(Card card,List<Card> row)
        {
            List<Card> ownBattefield = new List<Card>();
            ownBattefield.AddRange(card.Owner.battleField.contactrow);
            ownBattefield.AddRange(card.Owner.battleField.distantrow);
            ownBattefield.AddRange(card.Owner.battleField.siegerow);
            int sum = card.Power;
            int avaragepower=0;
            int count = 0;
           
            foreach(Card Card in ownBattefield)
            { 
                if(Card.type != 1)
                {
                    sum += Card.Power; 
                    count ++;
                } 
            }
            if(count == 0)
            {
                card.Owner.totalforce = card.Power;
                return;
            }
            avaragepower = sum/(ownBattefield.Count()+1);

            foreach (Card Card in card.Owner.battleField.contactrow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.Owner.totalforce -= Card.Power;
                    Card.Power = avaragepower;
                    Card.Owner.totalforce += Card.Power;
                }
            }
             foreach (Card Card in card.Owner.battleField.distantrow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.Owner.totalforce -= Card.Power;
                    Card.Power = avaragepower;
                    Card.Owner.totalforce += Card.Power;
                }
            }
             foreach (Card Card in card.Owner.battleField.siegerow)
            {
                if(Card.type!=2 && Card.type!=1)
                {
                    Card.Owner.totalforce -= Card.Power;
                    Card.Power = avaragepower;
                    Card.Owner.totalforce += Card.Power;
                }
            }
            card.Power = avaragepower;
            card.Owner.totalforce+= card.Power;
        }

        //Carta senuelo: permite cambiar una carta de valor 0 por otra que este sobre la mesa desactivando los efectos ocasionados por la misma al existir
        public static void Decoy(Card card , List<Card> row)
        {
            RemoveEspecialCardEffect(card,row);
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
                card = enemyBattleField.OrderByDescending((card)=> card.Power).ToList().ElementAt(0); 
            else
                card = enemyBattleField.OrderBy((card)=> card.Power).ToList().ElementAt(0); 
            
            if (card.type==1)
            {
                enemyBattleField.Remove(card);
                return FindCardByPower(enemyBattleField,value);
            }
            return card;
        }

        public static void RemoveEspecialCardEffect (Card card, List<Card> row)
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
                    card.Owner.battleField.SetWeather(card.Owner.GetRow(card.row.First()),0);
                }
                else
                {
                    card.Owner.battleField.SetWeather(card.Owner.GetRow(card.row.First()),0);
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
                    Card.Owner.totalforce -= card.Power;
                    Card.Power -= card.Power;
                }
            }
        }
        private static void BeforeW_ReducePowerOfWeakCards(Card card, List<Card> row)
        {   //Si la carta tiene poder original menor que 4 le incrementa al poder actual de la carta su poder actual
            foreach (Card Card in row)
            {
                if(card.ID != Card.ID && Card.initialPowerAttack<=4 && Card.type!=2)
                {
                    Card.Power += Card.initialPowerAttack;
                    Card.Owner.totalforce += Card.initialPowerAttack; 
                }    
            }
        }
    }
}