using System.Security;

using System.ComponentModel;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting.Dependencies.NCalc;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections;
using UnityEngine.UI;
using UnityEditor.Build;
using GameLogic;



public class DeckManager: MonoBehaviour
{
    public  List<CardData> HumanityDeck;
    public  List<CardData> TitansDeck;
    public GameObject ClonedHand;
    private GameObject cardPrefab; 
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);
    public Player playerOnTurn;
    public PanelFader blurPanel;

    public DeckManager(CardLists cardLists)
    {
        HumanityDeck = cardLists.HumanityDeck;
        TitansDeck = cardLists.TitansDeck;
        cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
    }

     public void InitializeDeck(Player player)
     {
        SetDeck(player);
        Shuffle(player.deck);
        SpawnCards(player);
        SetDictionary(player);
     }
    
    //metodo Fisher Yates para barajear
    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, list.Count);
            //swap
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
    private void SpawnCards(Player player)
    {
        for (int i = 0; i < player.deck.Count; i++)
        {
            GameObject cardInstance = Instantiate(cardPrefab,player.board.deckcontainer.transform);
            cardInstance.transform.SetParent(player.board.deckcontainer.transform,false);
            cardInstance.transform.rotation = player.board.deckcontainer.transform.rotation;
            cardInstance.transform.localScale = cardScale;
            
            // Configura los datos de la carta en el prefab 
            CardDisplay display = cardInstance.GetComponent<CardDisplay>();
        
            if (display != null)
            {
                display.ApplyCardData(player.deck[i]);
            }
            player.CardInstances.Add(cardInstance);
        }
    }
    
     private void SetDeck(Player player)
    {
        if(player.faction=="Humanity")
        {
            foreach(CardData card in HumanityDeck)
            {
                CardData clone = card.Clone();
                clone.owner=player;
                player.deck.Add(clone);
            }
        }
        else
        {
            foreach(CardData card in TitansDeck)
            {   
                CardData clone = card.Clone();
                clone.owner=player;
                player.deck.Add(clone);
            }
        }
    }
    public void SetDictionary(Player player)
    {
        foreach (GameObject card in player.CardInstances)
        {
            if (!player.CardDictionary.ContainsKey(card.GetComponent<CardDisplay>().ID))
            {
                player.CardDictionary.Add((card.GetComponent<CardDisplay>()).ID, card);
            }
        }        
    }
    public void InitialCardChange(Player player, PanelFader blurPanel)
    {    
        playerOnTurn = player;
        this.blurPanel = blurPanel;
        blurPanel.FadeToOpaque();
        ClonedHand = Instantiate(player.board.handcontainer, new Vector3(-11,70,0), Quaternion.identity);
        ClonedHand.transform.SetParent(blurPanel.transform.parent,false);
        ClonedHand.transform.localScale = new Vector3(2.2f,2.2f,2.2f);
        
        foreach (Transform card in ClonedHand.transform)
        {
            if(card==ClonedHand.transform.GetChild(0))
                Destroy(card.gameObject); 
            card.gameObject.AddComponent<CardInteraction>(); 
            DragHandler dragHandler = card.gameObject.GetComponent<DragHandler>();
            CardHoverDisplay cardVisor = card.gameObject.GetComponent<CardHoverDisplay>(); 
            if (dragHandler != null)
            {
                dragHandler.enabled = false;
                cardVisor.enabled = false;
            }
   
        }
    }
} 



