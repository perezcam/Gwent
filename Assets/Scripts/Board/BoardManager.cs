using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using System.Diagnostics.Tracing;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject attackContainer;
    public GameObject IattackContainer;
    public GameObject distantContainer;
    public GameObject IdistntContainer;
    public GameObject siegeContainer;
    public GameObject IsiegeContainer;
    public List<GameObject> attackRow = new List<GameObject>();
    public List<GameObject> IattackRow = new List<GameObject>();
    public List<GameObject> distantRow = new List<GameObject>();
    public List<GameObject> IdistantRow = new List<GameObject>();
    public List<GameObject> siegeRow = new List<GameObject>();
    public List<GameObject> IsiegeRow = new List<GameObject>();
    public List<GameObject> graveyard = new List<GameObject>();
    public List<CardData> handlist = new List<CardData>();
    public GameObject handcontainer;
    public GameObject leaderSlot;
    public GameObject[] boostSlots = new GameObject[3];
    public GameObject deckcontainer;
    public GameObject graveYardContainer;
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
    // 1 para mostra fila destino y 0 para ocultarla
    public void ShoworHideRow (GameObject card,int option)
    {
        Color targetcolor = attackContainer.GetComponent<UnityEngine.UI.Image>().color;
        if (option == 1)
        {
            targetcolor.a = 0.2f;
        } 
        else
            targetcolor.a = 0;
       
        foreach (int row in card.GetComponent<CardDisplay>().cardData.rows)
        {
            switch (row)
            {
                case 1:
                    attackContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                case 3:
                    distantContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                case 5:
                    siegeContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                case 2:
                    IattackContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                case 4:
                    IdistntContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                case 6:
                    IsiegeContainer.GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                    break;
                // case Row.W_attack:
                //     GameManager.instance.WeatherRow.GetComponentAtIndex(0).GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                //     break;
                // case Row.W_distant:
                //     GameManager.instance.WeatherRow.GetComponentAtIndex(1).GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                //     break;
                // case Row.W_siege:
                //     GameManager.instance.WeatherRow.GetComponentAtIndex(2).GetComponent<UnityEngine.UI.Image>().color = targetcolor;
                //     bresak;       
            }
        }
    }

    public void Update()
    { 
        //Para que el jugador pueda jugar una sola carta
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