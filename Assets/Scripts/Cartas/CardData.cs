using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardData: ScriptableObject
{  
    public string cardName;
    public string description;
    public int attack;
    public Sprite imageSprite;
    public List<int> rows;
    public int currentRow;
    public int ID;
    public Player owner;
    public string faction;
    public void Initialize (string name,string faction,string description,int attack,int currentRow,List<int> rows,int ID,Player owner)
    {
        this.cardName=name;
        this.description=description;
        this.attack=attack;
        this.imageSprite= Resources.Load<Sprite>("Cartas/" + name);
        this.currentRow=currentRow;
        this.ID=ID;
        this.rows = rows;
        this.owner=owner;
        this.faction = faction;
    }

     public CardData Clone()
    {
        // Crear un nuevo objeto CardData
        CardData clone = ScriptableObject.CreateInstance<CardData>();

    
        clone.cardName = this.cardName;
        clone.description = this.description;
        clone.attack = this.attack;
        // Esto es  una referencia, no una copia el Sprite
        clone.imageSprite = this.imageSprite;
        clone.currentRow = this.currentRow;
        clone.ID = this.ID;
        clone.rows = this.rows;
        clone.owner = this.owner; 
        return clone;
    }
}

