using System.Security.Cryptography.X509Certificates;

namespace engine
{
    public enum Type
    {
        
        M = 1,
        R = 2,
        S = 3,
        MR = 12,
        RS=23,
        MRS=123,
    }
    public enum CardStatus  
    {
    Ondesk=0,
    Onhand=1,
    OnBattleField=2,
    OnGraveYard=3,
    }
    public enum Faction
    {
        ScoutingLegion = 0,
        MilitaryPolice = 1,
        StationaryGuard = 2,

    }
    public class Card
    {
        
        public Card(int powerattack, Type type, int cardId, Faction faction,CardStatus cardstatus)
        {
           this.powerattack=powerattack;
           this.cardId=cardId;
        }
        public int powerattack{get;set;}

        public int cardId{get;set;}
       
        public Type type{get;set;}
        public CardStatus cardStatus{get;set;}
        public Faction faction{get;set;}
        
       

       
    }

   
   
   
    public class Leader: Card
    {
         public Leader (int powerattack, Type type,int cardId, Faction faction, int wealth, Skill skill,CardStatus cardstatus)
            :base(powerattack, type,  cardId,faction,cardstatus)
        {

        }
        public enum Skill
        {
            //Exitos de habilidades especiales
        }

    }
   
      public class Heroe:Card
    {
     
        public Heroe (int powerattack, Type type,int cardId,Faction faction,CardStatus cardstatus)
            :base(powerattack, type,  cardId,faction,cardstatus)
        {
             //Implementar funcion NotAfeccted que no permite que el heroe sea afectado positiva o negativamente
        }
    }

    public class lure: Card
    {
         public lure (int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus)
            :base(powerattack,type,cardId,faction,cardstatus)
        {

        }
    }

      public class clearence: Card
    {
         public clearence (int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus)
            :base(powerattack,type,cardId,faction,cardstatus)
        {

        }
    }

        public class weather: Card
    {
         public weather (int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus)
            :base(powerattack,type,cardId,faction,cardstatus)
        {

        }

        
    }
}