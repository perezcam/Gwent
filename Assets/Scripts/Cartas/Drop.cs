using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameLogic;

public enum Row
{
    attackRow,IattackRow,distantRow,IdistantRow,siegeRow,IsiegeRow,Null,Leader,W_attack,W_distant,W_siege,
}
public class Drop : MonoBehaviour, IDropHandler
{
    public Row row;
    public Player player;
    public List<GameObject> itemsDropped;
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();
        Player cardOwner = droppedCard.GetComponent<CardDisplay>().owner;
        CardData droppedCardData = droppedCard.GetComponent<CardDisplay>().cardData;
        droppedCard.GetComponent<CardDisplay>().owner.board.ShoworHideRow(droppedCard,0);

        switch (row)
        {
            case Row.attackRow:
                itemsDropped = player.board.attackRow;
                droppedCardData.currentRow = 1;
                break;
            case Row.distantRow:
                itemsDropped = player.board.distantRow;
                droppedCardData.currentRow = 3;
                break;
            case Row.siegeRow:
                itemsDropped = player.board.siegeRow; 
                droppedCardData.currentRow = 5;
                break;
            case Row.IattackRow:
                itemsDropped = player.board.IattackRow;
                droppedCardData.currentRow = 2;
                break;
            case Row.IdistantRow:
                itemsDropped = player.board.IdistantRow;
                droppedCardData.currentRow = 4;
                break;
            case Row.IsiegeRow:
                itemsDropped = player.board.IsiegeRow;
                droppedCardData.currentRow = 6;
                break;
        }
        
      
        if(cardOwner!= player || !droppedCardData.rows.Contains(DeckManager.GetRow(row))||((droppedCardData.currentRow == 2||droppedCardData.currentRow ==4||droppedCardData.currentRow ==6) && itemsDropped.Count==1)||itemsDropped.Count >= 7) 
        {
            droppedCard.transform.position = dragHandler.startPosition;
            droppedCard.transform.SetParent(dragHandler.startParent, false);
            droppedCard.transform.localScale = cardScale;
            return;
        }
        else   
        { 
            int droppedid = (droppedCard.GetComponent<CardDisplay>()).ID;
            if (droppedCard != null)
            {  
                GameLogic.Card logicCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[droppedid];
                player.hand.Remove(droppedCard);
                switch (row)
                {   
                    case Row.attackRow: 
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid, 1);
                        player.AddCardTo(player.board.attackRow,droppedCard);
                        break;
                    case Row.IattackRow:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,1);
                        player.AddCardTo(player.board.siegeRow,droppedCard);
                        break;
                    case Row.distantRow:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,2);
                        player.AddCardTo(player.board.distantRow,droppedCard);
                        break;
                    case Row.IdistantRow:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,2);
                        player.AddCardTo(player.board.siegeRow,droppedCard);
                        break;
                    case Row.siegeRow:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,3);
                        player.AddCardTo(player.board.siegeRow,droppedCard);
                        break;
                    case Row.IsiegeRow:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,3);
                        player.AddCardTo(player.board.siegeRow,droppedCard);
                        break;   
                    case Row.Null:
                        return;  
                }
                
            }
            // Configura el objeto como hijo del contenedor y ajusta su posición
            droppedCard.transform.SetParent(transform);
            droppedCard.transform.position = transform.position;
            droppedCard.transform.localScale = cardScale;
            
            if (dragHandler != null)
            {
                dragHandler.enabled = false;
            }
            itemsDropped.Add(droppedCard);
        }
    }
}

