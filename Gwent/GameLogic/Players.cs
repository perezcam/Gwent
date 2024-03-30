using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace engine
{
  
    public class Player
    {
        public Player(string nick,Faction faction)
        {
            this.nick=nick;    
            this.faction=faction;
        }
        public bool Passed = false;
        public  bool OnTurn=false;
        public Faction faction;
        public string nick{get;set;}
        public int totalforce {get;set;}
        public bool roundwinner{get;set;}
        public  Card[] hand = new Card [10];
        public  Card[] deck = new Card [25];
        public BattleField battleField = new BattleField();
        
        public void AddtoDeck(Card card)
        {
            AddCardTo(card,deck);
        }
    
        public void InitializeHand ()
        {
            Completehand(hand);
        } 
        public void InitialChange(Card card1, Card card2) 
        {
             AddCardTo(card1,deck);
             DeleteFromHand(card1);
             card1.cardStatus=CardStatus.Ondeck;
             AddCardTo(card2,deck);
             DeleteFromHand(card2);
             card2.cardStatus=CardStatus.Ondeck;
             Completehand(hand);
        }
        public void Completehand(Card[] hand)
        {
            Random random = new Random();
          
           for (int i = 0; i < hand.Length; i++)
           {
                //Complete hand
                int target = random.Next(0,25);
                if(hand[i]==null)
                {   
                    while (deck[target]==null)
                    {
                        target = random.Next(0,25);
                    }
                    hand[i]= deck[target]; 
                    deck[target]=null!;
                    hand[i].cardStatus=CardStatus.Onhand;
                } 
            }
        }

         public void AddtoBattleField(Card card, Row row)
       {
            
            switch (row)
            {
                case Row.Contact:
                   AddCardTo(card, battleField.contactrow);
                     ActiveCard(card,battleField.contactrow);
                    break;
                case Row.Distant:
                   AddCardTo(card,battleField.distantrow);
                     ActiveCard(card,battleField.distantrow);
                    break;
                case Row.Siege:
                   AddCardTo(card,battleField.siegerow);
                     ActiveCard(card,battleField.siegerow);
                    break;
                case Row.Leader:
                    AddCardTo(card,battleField.leader);
                     ActiveCard(card,battleField.leader);
                    break;
                case Row.Wheather:
                    AddCardTo(card,battleField.weatherow);
                     ActiveCard(card,battleField.weatherow);
                    break;    
            }
            card.cardStatus = CardStatus.OnBattleField;
       }
  
            //Add the card to a specific position and if is not possible return false
         public  bool AddCardTo(Card card, Card[] cardsrow)
       {
            for (int i = 0; i < cardsrow.Length; i++)
            {
                if(cardsrow[i] == null)
                {
                    cardsrow[i] = card;
                    DeleteFromHand(card);
                    return true;           
                }
            }
            return false;
       }
         public void DeleteFromHand(Card card)
         {
            for (int i = 0; i < 10; i++)
            {
                if(hand[i]==card)
                hand[i]=null!;
            }
         }
         public void ActiveCard(Card card,Card[]row)
       {
           if(card.powerattack==0)
           {    

           }
           else
           {
                ((SilverCard)card).function(card,row);
                totalforce+=card.powerattack;
           }
       }
       public Player GetEnemy()
       {
           Player enemy = Project.Game.player1;
           if (this==Project.Game.player1)
           {
                enemy=Project.Game.player2;
           }
           return enemy;
       }
    }
} 