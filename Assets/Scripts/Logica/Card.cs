using System;
using System.Buffers;
using System.Security.Cryptography.X509Certificates;

namespace GameLogic
{
    // type 0 => carta de unidad, type 1=> carta especial type 2 => carta heroe, type 4=> decoy card
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
           
        }
        public int type{get;set;}
        public string name{get;set;}
        public Player owner{get;set;}
        public int powerattack{get;set;}
        public int ID{get;set;}
        public int row{get;set;}      
        public Functions cardfunction = new Functions();
        public int initialPowerAttack{get;set;} 
         
         public Card Clone()
        {
            Card clone = new Card(this.name, this.powerattack, this.ID, this.owner, this.cardfunction.function, this.row,this.type);
            clone.row = row;
            clone.ID = ID;
            clone.name = name;
            clone.initialPowerAttack = initialPowerAttack;
            clone.type = type;
            return clone;
        }
    }


}