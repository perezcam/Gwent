using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cardPrefab; 
    private GameObject instantiatedCard;
    public GameObject CardVisor;
    public Vector3 originalScale = new Vector3();
    public void OnPointerEnter(PointerEventData eventData)
    {
        CardVisor = GameObject.FindGameObjectWithTag("CardVisor");
        // originalScale = gameObject.transform.localScale;
        // gameObject.transform.localScale = new Vector3(0.03f,0.03f,0.03f);
        if (instantiatedCard == null)
        {
            instantiatedCard = Instantiate(cardPrefab, CardVisor.transform.position, Quaternion.identity);
            instantiatedCard.transform.SetParent(CardVisor.transform,true);
            instantiatedCard.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);
            CardDisplay originaldisplay = GetComponent<CardDisplay>();
            CardDisplay newCardDisplay = instantiatedCard.GetComponent<CardDisplay>();
            CopyCardDisplayData(originaldisplay,newCardDisplay);
            instantiatedCard.GetComponent<DragHandler>().enabled=false;
        }
    }

    private void CopyCardDisplayData(CardDisplay original, CardDisplay copy)
    {
        copy.cardname.text =  original.cardname.text;
        copy.description.text = original.description.text;
        copy.attackvalue.text = original.attackvalue.text;
        copy.cardImage.sprite =  original.cardImage.sprite;
        copy.row=original.row;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
 //       gameObject.transform.localScale = originalScale;
        if (instantiatedCard != null)
        {
            Destroy(instantiatedCard);
            instantiatedCard = null;
        }
    }
}