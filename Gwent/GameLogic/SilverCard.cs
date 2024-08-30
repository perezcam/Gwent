using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.CompilerServices;
using System.Diagnostics.Metrics;
namespace engine
{
    public delegate void SpecialFunction(Card card,Card[]row); 
    public class SilverCard:Card
    {
        public SilverCard (string name,int powerattack, Type type,int cardId,Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
            :base(name,powerattack, type, cardId,faction,cardstatus,player,function,row)
        {
            this.function=function;
        }
        public SpecialFunction function;
        // Increase the power of cards in a row
        public static void IncreasePowerRow(Card card,Card[]row) 
        {
            double incrementvalue=0;
            //Check if exist another card with the same active function
            bool exists= row.Any((card)=>((SilverCard)card).function==IncreasePowerRow); 
            
            for (int i = 0; i < 10; i++)
            {
                if(exists)
                {
                    incrementvalue+=(row[i].powerattack*0.75)-row[i].powerattack;
                }
                else
                {
                    incrementvalue += (row[i].powerattack*0.25)-row[i].powerattack;
                }
            }
        }

        // Remove the card with the most power from the enemy row where it is placed
      
        public static void DelCard (Card card,Card[] row)
        {    
            Player enemy=card.owner.GetEnemy();            
            Card[] enemyrow= new Card[9];
            //Select the row of the enemy
            switch (card.row)
            {
                case Row.Contact:
                    enemyrow= enemy.battleField.contactrow;
                    break;
                case Row.Distant:
                    enemyrow= enemy.battleField.distantrow;
                    break;
                case Row.Siege:
                    enemyrow= enemy.battleField.siegerow;
                    break;
            }
            Card[] copyenemyrow = enemyrow.OrderByDescending((card)=>card.powerattack).ToArray();
            //Send the powerfull enemy card to graveyard
            for (int i = 0; i < 24; i++)
            {
                if (enemy.battleField.graveyard[i]==null)
                    enemy.battleField.graveyard[i]=copyenemyrow[0];
            }
            
            for (int i = 0; i < 9; i++)
            {
                if(enemyrow[i].cardId==copyenemyrow[0].cardId)
                enemyrow[i]=null!;
            }
            
        }    
        // Allows you to draw the selected card
        public void StealCard(Card card, Card[]cardrow)
        {
            //no se como recibir la carta a robar 
            
            //Si la carta robada ya ha sido sel. un turno anterior no
            //permite ser robada.
          
        }
        // Multiplies the attack power of the card by the number of cards
        //on the board
        public void AttackMultiplier(Card card,Card[]cardsrow)
        {
            Player enemy = card.owner.GetEnemy();
            int totalcards=0;
            for (int i = 0; i < 9; i++)
            {
                if (card.owner.battleField.contactrow[i].type==card.type&&enemy.battleField.contactrow[i].type==card.type)
                totalcards++;
                else if(card.owner.battleField.distantrow[i].type==card.type&&enemy.battleField.distantrow[i].type==card.type)
                totalcards++;
                else if(card.owner.battleField.siegerow[i].type==card.type&&enemy.battleField.siegerow[i].type==card.type)
                totalcards++;
            }
            card.powerattack=card.powerattack*totalcards;
            card.owner.totalforce+=card.powerattack;
        }
        
       //Clear the enemy's field row with fewer cards
         public void DelRow (Card card,Card[]cardsrow)
        {
            Player enemy=card.owner.GetEnemy();
            Card[][] enemyboard = new Card [2][];
            enemyboard[0]= enemy.battleField.contactrow;
            enemyboard[1]= enemy.battleField.distantrow;
            enemyboard[2]= enemy.battleField.siegerow;
            
            enemyboard = enemyboard.OrderBy((arr)=>arr.Count()).ToArray();
            
            int index=0; 
            for (int i = 0; i < 25; i++)
            {
               if (enemy.battleField.graveyard[i]!=null)
               index++;
            }
            for (int i = 0; i < 9; i++)
            {   
                 if(enemyboard[0][i]!=null)
                 {
                 enemy.battleField.graveyard[index]=enemyboard[0][i];
                 index++;
                 enemyboard[0][i].cardStatus=CardStatus.OnGraveYard;
                 enemyboard[0][i]=null!;
                 }
            }            
        }
        // Calculates the average of the rival cards and equals it to the 
        //power of the own cards
        public void AveragedPower(Card card,Card[]cardsrow)
        {
            Player enemy = card.owner.GetEnemy();
            int totalpowerattack=0;
            int cardcounter=0;
            //Calculates the average
            for (int i = 0; i < 9; i++)
            {
                if(enemy.battleField.contactrow[i]!=null)
                {
                     totalpowerattack+=enemy.battleField.contactrow[i].powerattack;
                     cardcounter++;
                }
                if(enemy.battleField.distantrow[i]!=null)
                {
                     totalpowerattack+=enemy.battleField.distantrow[i].powerattack;
                     cardcounter++;
                }
                if(enemy.battleField.siegerow[i]!=null)
                {
                     totalpowerattack+=enemy.battleField.siegerow[i].powerattack;
                     cardcounter++;
                }
            }
            int average = totalpowerattack%cardcounter;
             //Equals the average powerattack of the enemy to the own in each card
             for (int i = 0; i < 9; i++)
             {
                if(card.owner.battleField.contactrow[i]!=null)
                {
                card.owner.totalforce-=card.owner.battleField.contactrow[i].powerattack;
                card.owner.battleField.contactrow[i].powerattack= average;
                card.owner.totalforce+=average;
                }
                if(card.owner.battleField.distantrow[i]!=null)
                {
                    card.owner.totalforce-=card.owner.battleField.distantrow[i].powerattack;
                    card.owner.battleField.distantrow[i].powerattack= average;   
                    card.owner.totalforce+=average;            
                } 
                if(card.owner.battleField.siegerow[i]!=null)
                {
                    card.owner.totalforce-=card.owner.battleField.siegerow[i].powerattack;
                    card.owner.battleField.siegerow[i].powerattack= average;
                    card.owner.totalforce+=average;  
                }
             }
        }


      




        




    }
}