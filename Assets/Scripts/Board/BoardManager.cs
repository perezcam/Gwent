using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<GameObject> attackRow = new List<GameObject>();
    public List<GameObject> distantRow = new List<GameObject>();
    public List<GameObject> siegeRow = new List<GameObject>();
    public List<GameObject> weatherRow = new List<GameObject>();
    public List<CardData> handlist = new List<CardData>();
    public GameObject handcontainer;
    public GameObject leaderSlot;
    public GameObject[] boostSlots = new GameObject[3];
    public GameObject deckcontainer;
    public bool cardPlayed = true;

    public void Update()
    { 
        //Para que el jugador pueda jugan una sola carta
        if (cardPlayed)
        {
            foreach (Transform cardinhand in handcontainer.transform)
            {
            DragHandler dragHandler = cardinhand.GetComponent<DragHandler>();
                if (dragHandler != null)
                {
                    dragHandler.enabled = false;
                }
            }
        }
        else
        {
            foreach (Transform cardinhand in handcontainer.transform)
            {
            DragHandler dragHandler = cardinhand.GetComponent<DragHandler>();
                if (dragHandler != null)
                {
                    dragHandler.enabled = true;
                }
            }
        }
    }
   

}