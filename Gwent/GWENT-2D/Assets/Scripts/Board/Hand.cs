using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {   
        Debug.Log (GameManager.instance.deckManager.HumanityDeck);
        Debug.Log ("adios");
        GameObject itemBeingDragged = DragHandler.itemDragging;

        // Verifica si hay un objeto siendo arrastrado(DICEN Q ES NECESARIO Y PREVENTIVO PREGUNTARRRRR)
        if (itemBeingDragged != null)
        {   
            // Cambia el padre del objeto arrastrado a este transform
            itemBeingDragged.transform.SetParent(transform);
        }
    }
}
