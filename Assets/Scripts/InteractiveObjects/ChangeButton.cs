using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButton : MonoBehaviour
{
    public GameManager game;
    public void OnChangeHandButtonClick()
    {
         List<GameObject> CardsToDelete = new List<GameObject>();
        foreach (CardInteraction selectedCard in CardInteraction.selectedCards)
        {
            CardDisplay selectedCardData = selectedCard.GetComponent<CardDisplay>();
           
            List<GameObject> instancesToAdd = new List<GameObject>();

            // Encuentra las coincidencias para mover y eliminar
            int index = 0;
            foreach (Transform cardTransform in game.deckManager.playerOnTurn.board.handcontainer.transform)
            {   
                if(index==0)
                {
                    index++;
                    continue;
                }
                else
                {
                    GameObject cardData = cardTransform.gameObject;
                    if (cardData != null && selectedCardData != null && cardData.GetComponent<CardDisplay>().ID == selectedCardData.ID)
                    {
                        foreach (GameObject card in game.deckManager.playerOnTurn.hand)
                        {
                            if (cardData.GetComponent<CardDisplay>().ID == card.GetComponent<CardDisplay>().ID)
                            {
                                instancesToAdd.Add(card);
                                CardsToDelete.Add(card);
                            }
                        }
                    }

                    foreach (GameObject card in instancesToAdd)
                    {
                        game.deckManager.playerOnTurn.CardInstances.Add(card);
                    }
                    foreach (GameObject card in CardsToDelete)
                    {
                        game.deckManager.playerOnTurn.hand.Remove(card);
                    }
                }
            }   
        }
        foreach (GameObject cardToRemove in CardsToDelete)
        { 
            Destroy(cardToRemove);
        }
        CardInteraction.selectedCards.Clear();
        game.deckManager.playerOnTurn.Completehand(10);
        game.blurPanel.gameObject.SetActive(false);
        Destroy(game.deckManager.ClonedHand);
    }
}