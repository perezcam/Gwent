using UnityEngine;
using System.Collections.Generic;


public class DeckManager 
{
    public  List<CardData> HumanityDeck = new List<CardData>();
    public  List<CardData> TitansDeck = new List<CardData>();

    public DeckManager()
    {
        Shuffle(HumanityDeck);
        Shuffle(TitansDeck); 
    }
        //metodo Fisher Yates para barajear
     void Shuffle<T>(List<T> list)
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
}
