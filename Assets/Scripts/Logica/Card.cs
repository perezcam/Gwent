using System;
using System.Buffers;
using System.Security.Cryptography.X509Certificates;

namespace GameLogic
{
    
    public class Card
    {
        public Card(string name,int powerattack, int ID,Player player,SpecialFunction function,int row)
        {
           this.powerattack=powerattack;
           this.ID=ID;
           this.owner=player;
           this.name=name;
           this.row = row;
           cardfunction.function=function;
           initialPowerAttack = powerattack;
           On_W_ReducePowerOfWeakCards = false;
           On_W_ResetCardValues = false;
        }

        public string name{get;set;}
        public Player owner{get;set;}
        public int powerattack{get;set;}
        public int ID{get;set;}
        public int row{get;set;}      
        public Functions cardfunction = new Functions();
        public int initialPowerAttack{get;set;} 
        public bool On_W_ReducePowerOfWeakCards{get;set;}
        public bool On_W_ResetCardValues{get;set;}
         
         public Card Clone()
        {
            Card clone = new Card(this.name, this.powerattack, this.ID, this.owner, this.cardfunction.function, this.row);
            clone.row = row;
            clone.ID = ID;
            clone.name = name;
            clone.initialPowerAttack = initialPowerAttack;
            clone.On_W_ReducePowerOfWeakCards = On_W_ReducePowerOfWeakCards;
            clone.On_W_ResetCardValues = On_W_ResetCardValues;
            return clone;
        }
    }


}