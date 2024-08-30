using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDisplay : MonoBehaviour
{
 // Scriptable Object de esta carta
    public CardData cardData;
    public TextMeshProUGUI cardname;
    public Image cardBody;
     public TextMeshProUGUI description;
    public Image cardImage;
    public TextMeshProUGUI attackvalue;
    public int currentRow;
    public int ID;
    public Player owner;
    public void ApplyCardData(CardData cardData)
    {
        if (cardData != null) 
        {
            cardname.text = cardData.cardName;
            description.text = cardData.description;
            attackvalue.text = cardData.attack.ToString();
            cardImage.sprite = cardData.imageSprite;
            currentRow = cardData.currentRow;
            ID = cardData.ID;
            owner = cardData.owner;
            this.cardData = cardData;
        }
        else
        {
            Debug.LogWarning("CardData no asignado a " + gameObject.name);
        }
    }
}

