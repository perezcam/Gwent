using System;
using System.Buffers;
using System.Security.Cryptography.X509Certificates;

namespace GameLogic
{
    // type 0 => carta de unidad, tupe 1=> carta especial type 2 => carta heroe
    // row 1 => contact row 2 => distant  row3=> siege 
    
    public class Card
    {
        public Card(string name,int powerattack, int ID,Player player,SpecialFunction function,int row, int type)
        {
           this.powerattack=powerattack;
           this.ID=ID;
           this.owner=player;
           this.name=name;
           this.row = row;
           this.type = type;
           cardfunction.function=function;
           initialPowerAttack = powerattack;
           On_W_ReducePowerOfWeakCards = false;
           On_W_ResetCardValues = false;
           
        }
        public int type{get;set;}
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
            Card clone = new Card(this.name, this.powerattack, this.ID, this.owner, this.cardfunction.function, this.row,this.type);
            clone.row = row;
            clone.ID = ID;
            clone.name = name;
            clone.initialPowerAttack = initialPowerAttack;
            clone.On_W_ReducePowerOfWeakCards = On_W_ReducePowerOfWeakCards;
            clone.On_W_ResetCardValues = On_W_ResetCardValues;
            clone.type = type;
            return clone;
        }
    }


}