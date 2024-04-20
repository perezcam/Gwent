using System.Security.Principal;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace GameLogic
{
    
    public class BattleField
    {
        public BattleField()
        {
           
        }
        public  List<Card> contactrow = new List<Card>();
        public  List<Card> distantrow= new List<Card>();
        public  List<Card> siegerow= new List<Card>();
        public  List<Card> graveyard= new List<Card>();

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
                     contactrow[i]=null!;
                }
                if(distantrow[i]!=null)
                {
                     graveyard[index]=distantrow[i];
                     index++;
                     contactrow[i]=null!;
                }
                if(siegerow[i]!=null)
                {
                     graveyard[index]=siegerow[i];
                     index++;
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
                     contactrow[i]=null!;
                }

            } 
       }  
    }    
}