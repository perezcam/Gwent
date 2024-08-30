using System;
using System.Collections.Generic;
using GameLogic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    public Transform handTransform; 
    private GameLogic.Player logicPlayer;
    private Player player;
    private Player OnDropOwner;
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);
    public GameObject W_attack;
    public GameObject W_distant;
    public GameObject W_siege;

    public void OnDrop(PointerEventData eventData)
    {
            player = eventData.pointerDrag.GetComponent<CardDisplay>().owner;
            OnDropOwner = gameObject.GetComponent<CardDisplay>().owner;
       
        if (eventData.pointerDrag != null && player == OnDropOwner)
        {
            W_attack = GameManager.instance.WeatherRow.transform.GetChild(0).gameObject;
            W_distant = GameManager.instance.WeatherRow.transform.GetChild(1).gameObject;
            W_siege = GameManager.instance.WeatherRow.transform.GetChild(2).gameObject;
            CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
            GameObject droppedCard = eventData.pointerDrag; 
            Card logiCurrentCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[gameObject.GetComponent<CardDisplay>().ID];
            logicPlayer = logiCurrentCard.Owner;
            handTransform = droppedCard.GetComponent<DragHandler>().startParent;
            int cardID = cardDisplay.ID;
            Card logiCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[cardID];
            int cardtype = logiCard.type;
            

            // Verifica si la carta arrastrada es una carta tipo senuelo
            if (droppedCard != null && cardtype == 4 && player.playername == logicPlayer.nick)
            {
                //Coloca a la carta senuelo en el lugar de la carta
                droppedCard.transform.SetParent(transform.parent);
                droppedCard.transform.position = transform.position;
                if(droppedCard.transform.parent != W_attack.transform && droppedCard.transform.parent != W_distant.transform && droppedCard.transform.parent != W_siege.transform )
                    droppedCard.transform.localScale =  new Vector2(0.0244f,0.0255f);

                logiCard.row = logiCurrentCard.row;
                droppedCard.gameObject.GetComponent<DragHandler>().enabled = false;

                //Coloca la carta nuevamente en la mano
                transform.SetParent(handTransform);
                transform.position = handTransform.position; 
                transform.localScale = new Vector2(0.0244f,0.0255f);
                gameObject.GetComponent<CardDisplay>().attackvalue.text = logiCurrentCard.initialPowerAttack.ToString();

                //Elimina la carta del tablero de la logica y quita efectos provocado por esa carta en caso de existir
                List<Card> row = logicPlayer.GetRow(logiCurrentCard.currentRow);
                Functions.RemoveEspecialCardEffect(logiCurrentCard,row);
                Functions.RemoveEspecialCardEffect(logiCurrentCard,logicPlayer.GetEnemy().GetRow(logiCurrentCard.currentRow));
                player.UpdateCardsPower();
                logicPlayer.totalforce -= logiCurrentCard.Power;
                logicPlayer.GetRow(logiCurrentCard.currentRow).Remove(logiCurrentCard);
                logicPlayer.hand.Add(logiCurrentCard);
                if(player.cardClones.Contains(droppedCard))
                {
                    player.cardClones.Remove(droppedCard);
                    player.DeleteCards(new List<int>(){cardID},player);
                    
                }                
                //Agrega la carta a la mano virtual, la elimina del tablero y 
                //Agrega la carta senuelo en la logica
                player.hand.Add(gameObject);
                gameObject.GetComponent<CardDropHandler>().enabled = false;
                if(logiCard.currentRow==1)
                {
                    player.board.attackRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.currentRow);
                    player.AddCardTo(player.board.attackRow,droppedCard);
                }
                else if (logiCard.currentRow==2)
                {
                    player.board.distantRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.currentRow);
                    player.AddCardTo(player.board.distantRow,droppedCard);
                }
                else
                {
                    player.board.siegeRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.currentRow);
                    player.AddCardTo(player.board.siegeRow,droppedCard);
                }
                
            }    
            else
            {
                droppedCard.transform.SetParent(handTransform);
                droppedCard.transform.position = transform.position;
            }
            
        }      
        else if(eventData.pointerDrag != null)
        {
            handTransform = eventData.pointerDrag.GetComponent<DragHandler>().startParent;
            eventData.pointerDrag.transform.SetParent(handTransform);
            eventData.pointerDrag.transform.position = transform.position;
        }
    }
}