using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FactionSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform FinalPosition;
    public float scaleFactor = 3f; 
    private Vector3 originalScale;
    public  bool isSelected;
    private Vector3 finalScale;
    private Vector3 originalPosition;
    public string FactionName;

    void Start()
    {
        isSelected = false;
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }
    public void InitialStatus()
    {
        transform.localScale = originalScale;
        transform.position = originalPosition;
        gameObject.GetComponent<Image>().color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected=true;
        transform.position = FinalPosition.position;
        gameObject.GetComponent<Image>().color = Color.red;
        finalScale = originalScale*scaleFactor;
        eventData.pointerClick.GetComponent<FactionSelection>().enabled = false;
        eventData.pointerClick.transform.localScale = finalScale;
        
    }
    void Update ()
    { 
        
    }

}

