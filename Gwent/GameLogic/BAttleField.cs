using System.Security.Principal;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.CompilerServices;

namespace engine
{
    
    
    public class BattleField
    {
        public BattleField()
        {
           
        }
        public  Card[] contactrow = new Card [9];
        public  Card[] distantrow= new Card [9];
        public  Card[] siegerow= new Card[9];
        public  Card[] graveyard= new Card[25];
        public  Card[] leader = new Card [1];
        public  Card[] weatherow= new Card[3];

      //Abajo separe esa funcion en tres partes para poder usarlas en las funciones 
      //Si al final las uso la suigiente hay q cambiarla para q solo llame a las tres funciones
       public void SendtoGraveyard()
       {
            int index=0; 
            for (int i = 0; i < 25; i++)
            {
               if (graveyard[i]!=null)
               index++;
            }
            for (int i = 0; i < 9; i++)
            {
                if(contactrow[i]!=null)
                {
                     graveyard[index]=contactrow[i];
                     index++;
                     contactrow [i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }
                if(distantrow[i]!=null)
                {
                     graveyard[index]=distantrow[i];
                     index++;
                     distantrow[i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }
                if(siegerow[i]!=null)
                {
                     graveyard[index]=siegerow[i];
                     index++;
                     siegerow[i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }

            } 
       }

       public void SendCRowtoGraveyard()
       {
           int index=0; 
            for (int i = 0; i < 25; i++)
            {
               if (graveyard[i]!=null)
               index++;
            }
            for (int i = 0; i < 9; i++)
            {
                if(contactrow[i]!=null)
                {
                     graveyard[index]=contactrow[i];
                     index++;
                     contactrow [i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }
            }
       }
       public void SendDRowtoGraveyard()
       {
          int index=0; 
            for (int i = 0; i < 25; i++)
            {
               if (graveyard[i]!=null)
               index++;

            }
            
            for (int i = 0; i < 9; i++)
            {
               
                if(distantrow[i]!=null)
                {
                     graveyard[index]=distantrow[i];
                     index++;
                     distantrow[i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }
            }
       }
       public void SendSRowtoGraveyard()
       {
            int index=0; 
            for (int i = 0; i < 25; i++)
            {
               if (graveyard[i]!=null)
               index++;
            }
            for (int i = 0; i < 9; i++)
            {
                if(siegerow[i]!=null)
                {
                     graveyard[index]=siegerow[i];
                     index++;
                     siegerow[i].cardStatus=CardStatus.OnGraveYard;
                     contactrow[i]=null!;
                }

            } 
       }
       
         

        

                    
       
    }
    
    


    
}