using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;
using Unity.VisualScripting;

[System.Serializable] 
public class Player
{
    public string playerName;
    public List<CardData> deck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public CardData leader; 
    public int points; 
    public string faction;
    public Board board;


    // Método para inicializar el jugador.
    public void Initialize(string name, string faction)
    {
        this.playerName = name;
        this.faction = faction;
        SetDeck();
    }

    private void SetDeck()
    {
        if(faction=="Humanity")
            deck = new DeckManager().HumanityDeck;
        else
            deck = new DeckManager().TitansDeck;
    }
    public void Update()
    {

    }

}

