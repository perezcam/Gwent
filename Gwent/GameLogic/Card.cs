using System.Buffers;
using System.Security.Cryptography.X509Certificates;

namespace engine
{
    public enum Row
    {
        Null,
        Contact,
        Distant,
        Siege,
        Leader,
        Wheather,
    }
    public enum Type
    {
        
        M = 1,
        R = 2,
        S = 3,
        MR = 12,
        RS=23,
        MS=13,
        MRS=123,
    }
    public enum CardStatus  
    {
    Ondeck=0,
    Onhand=1,
    OnBattleField=2,
    OnGraveYard=3,
    }
    public enum Faction
    {
        Humanity,
        Titans,

    }
    public class Card
    {
        public Card(string name,int powerattack, Type type, int cardId, Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
        {
           this.powerattack=powerattack;
           this.cardId=cardId;
           this.owner=player;
           this.name=name;
        }
        public string name{get;set;}
        public Player owner{get;set;}
        public int powerattack{get;set;}
        public int cardId{get;set;}
        public Type type{get;set;}
        public CardStatus cardStatus{get;set;}
        public Faction faction{get;set;}
        public Row row{get;set;}
        
        public static void NullFunction (Card card,Card[]cardsrow)
        {
            throw new NotImplementedException();
            //solo la cree para q el listado de cartas no me de errores hasta q le asigne su funcion a cada carta
            //aunque esta tambien sera una EspecialFunction
        }
    }

    public class Leader: Card
    {
         public Leader (string name,int powerattack, Type type,int cardId, Faction faction, int wealth, Skill skill,CardStatus cardstatus,Player player,SpecialFunction function,Row row )
         :base(name,powerattack, type,  cardId,faction,cardstatus,player,function,row)
        {

        }        
        public enum Skill
        {
            //Exitos de habilidades especiales
        }
    }
      public class Heroe:Card
    {
        public Heroe (string name,int powerattack, Type type,int cardId,Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
            :base(name,powerattack, type,  cardId,faction,cardstatus,player,function,row)
        {
             //Implementar funcion NotAfeccted que no permite que el heroe sea afectado positiva o negativamente
        }
    }
    public class bait: Card
    {
         public bait (string name,int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
            :base(name,powerattack,type,cardId,faction,cardstatus,player,function,row)
        {
            this.powerattack=0;
        }
        
        public void BaitCard(Card card, Card[]row)
        {
            // 
        }
    }
      public class clearence: Card
    {
         public clearence (string name,int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
            :base(name,powerattack,type,cardId,faction,cardstatus,player,function,row)
        {

        }
    }
        public class weather: Card
    {
         public weather (string name,int powerattack,Type type, int cardId, Faction faction,CardStatus cardstatus,Player player,SpecialFunction function,Row row)
            :base(name,powerattack,type,cardId,faction,cardstatus,player,function,row)
        {

        }   
    }
}