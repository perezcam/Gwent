using UnityEditor;
using UnityEngine;

public class CardData: ScriptableObject
{  
    public string cardName;
    public string description;
    public int attack;
    public Sprite imageSprite;
    public Row row;
    public int ID;
    public Player owner;
    public void Initialize (string name,string description,int attack,Row row,int ID,Player owner)
    {
        this.cardName=name;
        this.description=description;
        this.attack=attack;
        this.imageSprite= Resources.Load<Sprite>("Cartas/" + name);
        this.row=row;
        this.ID=ID;
        this.owner=owner;
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
        clone.row = this.row;
        clone.ID = this.ID;
        clone.owner = this.owner; 
        return clone;
    }
}

