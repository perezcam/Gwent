using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;

public class CardLists
{
    public  List<CardData> HumanityDeck;
    public  List<CardData> TitansDeck;
    public CardLists(Dictionary<int,Card> LogicCardDictionary)
    {
        HumanityDeck = new List<CardData>();
        TitansDeck= new List<CardData>();
        foreach (var element in LogicCardDictionary)
        {
            Card card = element.Value;
            CardData cardData = ScriptableObject.CreateInstance<CardData>();
            cardData.Initialize(card.Name,card.Faction,card.description,card.Power,card.currentRow,card.row,card.ID,null);
            if(cardData.faction == "Titans")
            {
                TitansDeck.Add(cardData);
            }
            else
                HumanityDeck.Add(cardData);
        }
    }
}
