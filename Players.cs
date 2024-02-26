using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.ComponentModel;

namespace engine
{
  
    public class Player
    {
        public Player(string nick,int totalforce, Faction faction)
        {
            this.nick=nick;
            this.totalforce=totalforce;
        }
        public string nick{get;set;}
        public int totalforce {get;set;}
        public bool roundwinner{get;set;}
        Card[] hand = new Card [10];
        Card[] desk = new Card [25];
        
        public void AddtoDesk(Card card)
        {
            Game.AddCardTo(desk,card);
        }
        
        public void InitializeHand ()
        {
           Random random = new Random();
           int[]targetarr = new int[10];
           
           for (int i = 0; i < 10; i++)
           {
                int target = random.Next(0,24);
                for (int e = 0; e < 10; e++)
                {
                    if(targetarr[e]==target)
                    target=random.Next(0,24);
                    else
                    break;
                }
                targetarr[i]=target;
                hand[i]= desk[target]; 
           }
        } 

        public void InitialChange(Card card1, Card card2) 
        {
            
        }
    

    }
 
 
}