using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{  
    public string cardName;
    public string description;
    public int attack;
    public Sprite imageSprite;
}

