using System;
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
    public static bool isSelected;
    private Vector3 finalScale;
    private Vector3 originalPosition;
    public string FactionName;


    void Start()
    {
        isSelected = false;
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
        imageToDestroy.SetActive(false);
        targetImage.position = FinalPosition.position;
        targetImage.GetComponent<Image>().color = Color.red;
        isSelected=true;
        finalScale = originalScale*scaleFactor;
        targetImage.localScale=finalScale;
        if(isSelected)
         {
             //targetImage.localScale = finalScale;
             if (SetGameScene.instance.faction1=="")
                SetGameScene.instance.faction1= FactionName;
             else
                SetGameScene.instance.faction2=FactionName;
         }
        
    }
    void Update ()
    { 
         
         if(Input.GetKey(KeyCode.Escape))
         {
            //imageToDestroy.SetActive(true);
            targetImage.localScale = originalScale;
            isSelected = false;
            targetImage.position = originalPosition;
            targetImage.GetComponent<Image>().color=Color.white;
         }
    }

}

