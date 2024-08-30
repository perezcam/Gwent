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
        // On_W = 0 no hay clima, On_W =1 clima tipo 1 , On_W = 2 clima tipo 2
        public int On_W_attackrow;
        public int On_W_distantrow;
        public int On_W_siegerow;
     
       public List<Card>[] CardsonBattleField()
      {
          List <Card>[] totalBattleField = new List<Card>[3];
          totalBattleField[0] = contactrow;
          totalBattleField[1] = distantrow;
          totalBattleField[2] = siegerow;
          //Restaura los climas en caso de existir a su posicion original
           On_W_attackrow = 0;
           On_W_distantrow = 0;
           On_W_siegerow = 0;
          return totalBattleField;
      } 
      public List<Card> AllCard()
      {
            List<Card> field = new List<Card>();
            field.AddRange(contactrow);
            field.AddRange(distantrow);
            field.AddRange(siegerow);
            return field;
      }
       public void SetWeather (List<Card> row, int type)
       {
          if(row == contactrow)
               On_W_attackrow = type;
          else if (row==distantrow)
               On_W_distantrow = type;
          else
               On_W_siegerow = type;
       }
       public int GetWeather (List<Card> row)
       {
          if(row == contactrow)
              return On_W_attackrow ;
          else if (row==distantrow)
              return On_W_distantrow;
          else
              return On_W_siegerow;
       }
    }    
}