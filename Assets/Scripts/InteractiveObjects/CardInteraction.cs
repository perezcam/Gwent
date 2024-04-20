using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    public bool isSelected = false;
    private static int selectedCount = 0;
     public static List<CardInteraction> selectedCards = new List<CardInteraction>();
    void Awake()
    {
        originalScale = transform.localScale; 
        selectedCount = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) 
        {
            transform.localScale = originalScale * 1.5f; 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) 
        {
            transform.localScale = originalScale;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected)
        {
            isSelected = false;
            selectedCount--; 
            transform.localScale = originalScale;    
        }
        else if (selectedCount < 2) 
        {
            isSelected = true;
            selectedCount++; 
            selectedCards.Add(this);
        }
    }
}
