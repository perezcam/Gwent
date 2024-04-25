using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static GameObject itemDragging;
    public Vector3 startPosition;
    public Transform startParent;
    Transform potentialDragParent;
   
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemDragging = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        potentialDragParent = startParent.Find("DragParent"); 
        Row itemrow = itemDragging.GetComponent<CardDisplay>().row;
        if (potentialDragParent != null)
        {
            transform.SetParent(potentialDragParent);
        }
        if(itemrow == Row.W_attack || itemrow == Row.W_distant || itemrow == Row.W_siege)
        {
            GameManager.instance.WeatherRow.SetActive(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
        //Limpia la referencia estática al elemento arrastrado, señalando el fin de la operación de arrastre.
        GameManager.instance.WeatherRow.SetActive(false);
        itemDragging = null;
        if (transform.parent == potentialDragParent)
        {
            transform.position= startPosition;
            transform.SetParent(startParent,false);
        } 
  
    }
    void Update()
    {

    }
}
