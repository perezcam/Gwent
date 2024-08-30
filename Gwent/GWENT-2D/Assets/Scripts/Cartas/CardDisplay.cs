using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class CardDisplay : MonoBehaviour
{
    public CardData cardData; // Referencia al Scriptable Object de esta carta
    public TextMeshProUGUI cardname;
    public Image cardBody;
    public TextMeshProUGUI powerattack; 
    public TextMeshProUGUI description;
    public Image cardImage;
    public TextMeshProUGUI attackvalue;

    public void ApplyCardData(CardData cardData)
    {
        if (cardData != null) // Verifica si cardData ha sido asignado
        {
            cardname.text = cardData.cardName;
            description.text = cardData.description;
            attackvalue.text = cardData.attack.ToString();
            cardImage.sprite = cardData.imageSprite;
            Debug.LogWarning("entro if" + gameObject.name);
        }
        else
        {
            Debug.LogWarning("CardData no asignado a " + gameObject.name);
        }
    }
}

