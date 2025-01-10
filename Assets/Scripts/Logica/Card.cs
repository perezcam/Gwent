using System;
using System.Buffers;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace GameLogic
{
    // type 0 => carta de unidad, type 1=> carta especial type 2 => carta heroe,type 3 => carta lider ,type 4=> decoy card type 5 => weathercard
    // row 1 => contact row 2 => distant  row3=> siege row0 => hand row4 => graveyard row5 => deck row7 => Wattack row8 => Wdistant row9 => Wsiege
    
    public class Card
    {
        public Card(string Name,string Faction,string description,int Power, int ID,Player player,SpecialFunction function,List<int> row, int type)
        {
           this.Power=Power;
           this.ID=ID;
           this.Owner=player;
           this.Name=Name;
           this.description = description;
           this.Faction = Faction;
           this.row = row;
           this.type = type;
           this.currentRow = currentRow;
           cardfunction.function=function;
           initialPowerAttack = Power;
           
        }
        public int currentRow{get;set;}
        public int type{get;set;}
        public string Name{get;set;}
        public string Faction{get;set;}
        public string description{get;set;}
        public Player Owner{get;set;}
        public int Power{get;set;}
        public int ID{get;set;}
        public List<int> row{get;set;}      
        public Functions cardfunction = new Functions();
        public int initialPowerAttack{get;set;} 
         
         public Card Clone()
        {
            Card clone = new Card(this.Name,this.Faction ,this.description,this.Power, this.ID, this.Owner, this.cardfunction.function, this.row,this.type);
            clone.row = row;
            clone.ID = ID;
            clone.Name = Name;
            clone.initialPowerAttack = initialPowerAttack;
            clone.type = type;
            clone.currentRow = currentRow;
            clone.Faction = Faction;
            return clone;
        }
    }


}