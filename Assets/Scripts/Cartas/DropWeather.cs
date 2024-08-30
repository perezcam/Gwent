using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropWeather : MonoBehaviour, IDropHandler
{
     public Row row;
     private Vector2 cardScale = new Vector2(0.0244f,0.0255f);
     public bool ispossible;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();
        int cardrow = droppedCard.GetComponent<CardDisplay>().currentRow;
        Player cardOwner = droppedCard.GetComponent<CardDisplay>().owner;
        CardData droppedCardData =  droppedCard.GetComponent<CardDisplay>().cardData;
        droppedCard.GetComponent<CardDisplay>().owner.board.ShoworHideRow(droppedCard,0);
        
        if(row == Row.W_attack && !GameManager.instance.weatherows[0])
            ispossible = true;
        else if(row == Row.W_distant && !GameManager.instance.weatherows[1])
        {
            ispossible = true;
        }
        if(row == Row.W_siege && !GameManager.instance.weatherows[2])
            ispossible = true;
    
        if(!ispossible || !droppedCardData.rows.Contains(DeckManager.GetRow(row))) 
        {
            droppedCard.transform.position = dragHandler.startPosition;
            droppedCard.transform.SetParent(dragHandler.startParent, false);
            droppedCard.transform.localScale = cardScale;
            return;
        }
        else
        {
             int droppedid = droppedCard.GetComponent<CardDisplay>().ID;
             
             if (droppedCard != null)
            { 
                switch (row)
                {   
                    case Row.W_attack:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,1);
                        cardOwner.AddCardTo(cardOwner.board.attackRow,droppedCard);
                        droppedCardData.currentRow = 7;
                        GameManager.instance.weatherows[0] = true;
                        break;
                    case Row.W_distant:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,2);
                        cardOwner.AddCardTo(cardOwner.board.distantRow,droppedCard);
                        droppedCardData.currentRow = 8;
                        GameManager.instance.weatherows[1] = true;
                        break;
                    case Row.W_siege:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,3);
                        cardOwner.AddCardTo(cardOwner.board.siegeRow,droppedCard);
                        droppedCardData.currentRow = 9;
                        GameManager.instance.weatherows[2] = true;
                        break;     
                }
                // Configura el objeto como hijo del contenedor y ajusta su posición
                droppedCard.transform.SetParent(transform);
                droppedCard.transform.position = transform.position;
               
                if (dragHandler != null)
                {
                    dragHandler.enabled = false;
                }
                GameManager.instance.WeatherRow.SetActive(false);
                ispossible = false;
                
            }
        }
    }
    
}
