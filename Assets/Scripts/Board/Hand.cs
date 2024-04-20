using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {   
        Debug.Log ("dropped");
        GameObject droppedcard = DragHandler.itemDragging;

        // Verifica si hay un objeto siendo arrastrado(DICEN Q ES NECESARIO Y PREVENTIVO PREGUNTARRRRR)
        if (droppedcard != null)
        {   
            // Cambia el padre del objeto arrastrado a este transform
            droppedcard.transform.SetParent(transform);
        }
    }
}
