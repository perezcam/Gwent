using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public static GameObject itemDragging;
    Vector3 startposition;
    Transform startParent;
    Transform dragParent;
    void Start()
    {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        itemDragging= gameObject;
        startposition = transform.position;
        startParent = transform.parent;
        transform.SetParent(dragParent);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.Log("OnEndDrag");
        //Limpia la referencia estática al elemento arrastrado, señalando el fin de la operación de arrastre.
        itemDragging = null;
        //Si el padre del elemento es todavía el dragParent, mueve el elemento de vuelta a su posición original y a su padre.
        // Esta lógica implica una validación básica de la validez del lugar donde se suelta (por ejemplo, solo permitiendo que el elemento se suelte en ciertas áreas)
        if (transform.parent == dragParent)
        {
            transform.position= startposition;
            transform.SetParent(startParent);
        } 
    }
    void Update()
    {

    }
}
