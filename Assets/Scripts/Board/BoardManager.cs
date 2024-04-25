using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public List<GameObject> attackRow = new List<GameObject>();
    public List<GameObject> IattackRow = new List<GameObject>();
    public List<GameObject> distantRow = new List<GameObject>();
    public List<GameObject> IdistantRow = new List<GameObject>();
    public List<GameObject> siegeRow = new List<GameObject>();
    public List<GameObject> IsiegeRow = new List<GameObject>();
    public List<CardData> handlist = new List<CardData>();
    public GameObject handcontainer;
    public GameObject leaderSlot;
    public GameObject[] boostSlots = new GameObject[3];
    public GameObject deckcontainer;
    public GameObject graveYard;
    public bool cardPlayed = true;
    
    public void ClearUIRows()
    {
        attackRow.Clear();
        distantRow.Clear();
        siegeRow.Clear();
        IsiegeRow.Clear();
        IdistantRow.Clear();
        IattackRow.Clear();
        GameManager.instance.weatherows[0] = false;
        GameManager.instance.weatherows[1] = false;
        GameManager.instance.weatherows[2] = false;
    }

    public void Update()
    { 
        //Para que el jugador pueda jugan una sola carta
        if (cardPlayed || deckcontainer.GetComponentInChildren<CardDisplay>().owner.passed)
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