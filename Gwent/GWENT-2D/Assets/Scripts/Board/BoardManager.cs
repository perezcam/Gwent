using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum Player
    {
        player1,
        player2
    }
    public List<GameObject> attackRow = new List<GameObject>();
    public List<GameObject> distantRow = new List<GameObject>();
    public List<GameObject> siegeRow = new List<GameObject>();
    public List<CardData> handcontainer = new List<CardData>();
    public List<GameObject> graveyard = new List<GameObject>();
    public GameObject leaderSlot;
    public GameObject deckcontainer;
    public GameObject[] boostSlots = new GameObject[3];
    
    public void AddCardTo(List<GameObject> row,GameObject card)
    { 
        row.Add(card);
    }
    public void Update()
    {
        
    }
    
}