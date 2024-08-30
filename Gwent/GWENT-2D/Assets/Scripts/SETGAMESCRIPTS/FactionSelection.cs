using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FactionSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform targetImage; 
    public GameObject imageToDestroy; 
    public RectTransform FinalPosition;
    public float scaleFactor = 3f; 
    private Vector3 originalScale;
    public static bool itSelected;
    private Vector3 finalScale;
    private Vector3 originalPosition;
    public string FactionName;


    void Start()
    {
        originalScale = targetImage.localScale;
        originalPosition = targetImage.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.localScale = originalScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // imageToDestroy.SetActive(false);
        targetImage.position = FinalPosition.position;
        finalScale = originalScale*scaleFactor;
        targetImage.GetComponent<Image>().color = Color.red;
        itSelected=true;
        
    }
    void Update ()
    { 
         if(itSelected)
         {
             targetImage.localScale = finalScale;
             faction1 = FactionName;
         }
         if(Input.GetKey(KeyCode.Escape))
         {
            //imageToDestroy.SetActive(true);
            targetImage.localScale = originalScale;
            itSelected = false;
            targetImage.position = originalPosition;
            targetImage.GetComponent<Image>().color=Color.white;
         }
    }

}

