using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IDropHandler
{
    public Player player; 
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);
    public void OnDrop(PointerEventData eventData)
    {   
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();

        if (droppedCard != null && droppedCard.gameObject.GetComponent<CardDisplay>().owner == player)
        {   
            // Cambia el padre del objeto arrastrado a este transform
            droppedCard.transform.SetParent(transform);
        }
        else
        {
            droppedCard.transform.position = dragHandler.startPosition;
            droppedCard.transform.SetParent(dragHandler.startParent, false);
            droppedCard.transform.localScale = cardScale;
            return;
        }
    }
}
