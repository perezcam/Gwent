using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    public bool isSelected = false;
    private static int selectedCount = 0;
    public static List<CardInteraction> selectedCards = new List<CardInteraction>();
    private int originalLayer;
    public AudioSource cardSelect;
    void Awake()
    {
        originalScale = transform.localScale; 
        selectedCount = 0;
        originalLayer = gameObject.layer;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) 
        {
            transform.localScale = originalScale * 1.5f; 
            transform.gameObject.layer = 20;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) 
        {
            transform.localScale = originalScale;
            transform.gameObject.layer = originalLayer;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        cardSelect = GameManager.instance.cardSelect;
        cardSelect.Play();
        if (isSelected)
        {
            isSelected = false;
            selectedCount--; 
            transform.localScale = originalScale;
            selectedCards.Remove(this); 
        }
        else if (selectedCount < 2) 
        {
            isSelected = true;
            selectedCount++; 
            selectedCards.Add(this); 
        }
    }

}
