using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardCompilerDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardPrefab; 
    private GameObject instantiatedCard;
    public GameObject CardVisor;
    public Card card;
    public Vector3 originalScale = new Vector3();
    public TextMeshProUGUI cardName;
    public void OnPointerEnter(PointerEventData eventData)
    {
        CardVisor.SetActive(true);
        Debug.Log("Cardivor is null?" + CardVisor is null);
        // originalScale = gameObject.transform.localScale;
        // gameObject.transform.localScale = new Vector3(0.03f,0.03f,0.03f);
        if (instantiatedCard == null)
        {
            instantiatedCard = Instantiate(cardPrefab, CardVisor.transform.position, Quaternion.identity);
            instantiatedCard.transform.SetParent(CardVisor.transform,true);
            instantiatedCard.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
            CardDisplay newCardDisplay = instantiatedCard.GetComponent<CardDisplay>();
            CopyCardDisplayData(card,newCardDisplay);
            instantiatedCard.GetComponent<DragHandler>().enabled=false;
        }
    }

    private void CopyCardDisplayData(Card original, CardDisplay copy)
    {
        copy.cardname.text =  original.Name;
        copy.description.text = original.description;
        copy.attackvalue.text = original.Power.ToString();
        Debug.Log(original.Name == "Beluga Azul" + "son iguales los nombres");
        copy.cardImage.sprite = Resources.Load<Sprite>("Cartas/" + original.Name);
        //copy.cardImage.sprite =  Resources.Load<Sprite>("Cartas/" + name);
        copy.currentRow=original.currentRow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //gameObject.transform.localScale = originalScale;
        if (instantiatedCard != null)
        {
            Destroy(instantiatedCard);
            instantiatedCard = null;
        }
        CardVisor.SetActive(false);
    }
}