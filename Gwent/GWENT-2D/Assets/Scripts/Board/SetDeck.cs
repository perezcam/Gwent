using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public GameObject cardPrefab; 
    public GameObject container;
    public List<CardData> cardDatas;

    void Start()
    {
        SpawnCards();
    }

    void SpawnCards()
    {
        for (int i = 0; i < cardDatas.Count; i++)
        {
            GameObject cardInstance = Instantiate(cardPrefab, container.transform, false);
            cardInstance.transform.localPosition = container.transform.localPosition;
            // Configura los datos de la carta en el prefab 
            CardDisplay display = cardInstance.GetComponent<CardDisplay>();
            if (display != null)
            {
                display.ApplyCardData(cardDatas[i]); 
            }
        }
    }
}
