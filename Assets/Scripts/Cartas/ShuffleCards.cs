using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShuffleCards : MonoBehaviour
{
    public List<GameObject> cards; 
    List<int> cardsOrder;
    public GridLayoutGroup gridLayoutGroup;
    public float shuffleDuration = 0.1f;

    public void UIShuffle(List<GameObject> cards, GameObject container, List<int> order)
    {
        this.cards = cards;
        this.gridLayoutGroup = container.GetComponent<GridLayoutGroup>();
        cardsOrder = order;
        StartCoroutine(Shuffle());
        Debug.Log("El tamaño de la mano es de " + cards.Count);
    }

    IEnumerator Shuffle()
    {
        List<GameObject> shuffledCards = new List<GameObject>(new GameObject[cardsOrder.Count]);

        // Reordenar las cartas según cardsOrder
        for (int i = 0; i < cards.Count; i++)
        {
            CardDisplay cardDisplay = cards[i].GetComponent<CardDisplay>();
            if (cardsOrder.Contains(cardDisplay.ID))
            {
                // Coloca las cartas en el orden correct
                int targetIndex = cardsOrder.IndexOf(cardDisplay.ID);
                shuffledCards[targetIndex] = cards[i];
            }
        }

        // Cambiar el orden de los elementos en la jerarquía
        for (int i = 0; i < shuffledCards.Count; i++)
        {
            GameObject targetCard = shuffledCards[i];
            if (targetCard != null)
            {
                // Mover la carta al nuevo lugar en la jerarquía del contenedor
                targetCard.transform.SetSiblingIndex(i);
                yield return new WaitForSeconds(shuffleDuration / shuffledCards.Count);
            }
        }
    }
}
