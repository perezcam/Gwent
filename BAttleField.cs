using System.Security.Principal;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.CompilerServices;

namespace engine
{
    public enum Row
    {
        Contact = 0,
        Distant =1,
        Siege=2,
    }
    
    public class BattleField
    {
        public BattleField(Player player)
        {
           this.player=player;
        }
        
        Player player;
        Card[] contactrow = new Card [9];
        Card[] distantrow= new Card [9];
        Card[] siegerow= new Card[9];
        Card[] graveyard= new Card[24];
        Card[] leader = new Card [1];


     
        public void AddtoBattleField(Card card, Row row)
       {
            switch (row)
            {
                case Row.Contact:
                    Game.AddCardTo(contactrow,card);
                    break;
                case Row.Distant:
                    Game.AddCardTo(distantrow,card);
                    break;
                case Row.Siege:
                    Game.AddCardTo(siegerow,card);
                    break;
            }
            ActiveCard(card);
            card.cardStatus = CardStatus.OnBattleField;

       }

     

       public void ActiveCard(Card card)
       {
           player.totalforce+=card.powerattack;
           if(card.powerattack==0)
           {
               //analizar tipo de funcion especial de la carta y activarla
           }
       }

       public void SendtoGraveyard()
       {
            int index=0; 
            for (int i = 0; i < 9; i++)
            {
                if(contactrow[i]!=null)
                {
                     graveyard[index]=contactrow[i];
                     index++;
                     contactrow [i].cardStatus=CardStatus.OnGraveYard;
                }
                if(distantrow[i]!=null)
                {
                     graveyard[index]=distantrow[i];
                     index++;
                     distantrow[i].cardStatus=CardStatus.OnGraveYard;
                }
                if(siegerow[i]!=null)
                {
                     graveyard[index]=siegerow[i];
                     index++;
                     siegerow[i].cardStatus=CardStatus.OnGraveYard;
                }

            } 
       }
         

        

                    
       
    }
    
    


    
}