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
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Card logiCurrentCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[gameObject.GetComponent<CardDisplay>().ID];
            logicPlayer = logiCurrentCard.owner;
            GameObject droppedCard = eventData.pointerDrag; 
            CardDisplay cardDisplay = eventData.pointerDrag.GetComponent<CardDisplay>();
            handTransform = droppedCard.GetComponent<DragHandler>().startParent;
            int cardID = cardDisplay.ID;
            Card logiCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[cardID];
            int cardtype = logiCard.type;
            player = logiCard.owner == GameManager.instance.logicGame.player1? GameManager.instance.player1 : GameManager.instance.player2;


            // Verifica si la carta arrastrada es una carta tipo senuelo
            if (droppedCard != null && cardtype == 3 && player.playername == logicPlayer.nick)
            {
                //Coloca a la carta senuelo en el lugar de la carta
                droppedCard.transform.SetParent(transform.parent);
                droppedCard.transform.position = transform.position;
                droppedCard.transform.localScale =  new Vector2(0.0244f,0.0255f);
                logiCard.row = logiCurrentCard.row;
                droppedCard.gameObject.GetComponent<DragHandler>().enabled = false;

                //Coloca la carta nuevamente en la mano
                transform.SetParent(handTransform);
                transform.position = handTransform.position; 
                transform.localScale = new Vector2(0.0244f,0.0255f);
                gameObject.GetComponent<CardDisplay>().attackvalue.text = logiCurrentCard.initialPowerAttack.ToString();

                //Elimina la carta del tablero de la logica y quita efectos provocado por esa carta en caso de existir
                List<Card> row = logicPlayer.GetRow(logiCurrentCard.row);
                Functions.RemuveSpecialCardsEfects(logiCurrentCard,row);
                Functions.RemuveSpecialCardsEfects(logiCurrentCard,logicPlayer.GetEnemy().GetRow(logiCurrentCard.row));
                player.UpdateCardsPower();
                logicPlayer.totalforce -= logiCurrentCard.powerattack;
                logicPlayer.GetRow(logiCurrentCard.row).Remove(logiCurrentCard);
                logicPlayer.hand.Add(logiCurrentCard);
                
                //Agrega la carta a la mano virtual, la elimina del tablero y 
                //Agrega la carta senuelo en la logica
                player.hand.Add(gameObject);
                gameObject.GetComponent<CardDropHandler>().enabled = false;
                if(logiCard.row==1)
                {
                    player.board.attackRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.row);
                    player.AddCardTo(player.board.attackRow,droppedCard);
                }
                else if (logiCard.row==2)
                {
                    player.board.distantRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.row);
                    player.AddCardTo(player.board.distantRow,droppedCard);
                }
                else
                {
                    player.board.siegeRow.Remove(gameObject);
                    logicPlayer.AddtoBattleField(logiCard.ID,logiCurrentCard.row);
                    player.AddCardTo(player.board.siegeRow,droppedCard);
                }
                
            }    
            else
            {
                droppedCard.transform.SetParent(handTransform);
                droppedCard.transform.position = transform.position;
            }
            
        }      
    }
}