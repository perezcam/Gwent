using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropWeather : MonoBehaviour, IDropHandler
{
     public Row row;
     private Vector2 cardScale = new Vector2(0.0244f,0.0255f);
     public List<GameObject> itemsDropped = new List<GameObject>();
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();
        Row cardrow = droppedCard.GetComponent<CardDisplay>().row;
        Player cardOwner = droppedCard.GetComponent<CardDisplay>().owner;
      
        if(itemsDropped.Count >= 1) 
        {
            droppedCard.transform.position = dragHandler.startPosition;
            droppedCard.transform.SetParent(dragHandler.startParent, false);
            droppedCard.transform.localScale = cardScale;
           
            return;
        }
        else
        {
             int droppedid = (droppedCard.GetComponent<CardDisplay>()).ID;
             Player player = GameManager.instance.playerOnTurn;
            
             if (droppedCard != null)
            {  //Revisar si por otro efecto se afecta una carta clima por la logica al agregarlo todo junto
                GameLogic.Card logicCard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[droppedid];
                GameLogic.BattleField battleField = GameManager.instance.logicGame.PlayerOnTurn().battleField;
                Debug.Log(logicCard.row);
                switch (row)
                {   
                    case Row.W_attack:
                        GameManager.instance.logicGame.PlayerOnTurn().ActiveCard(logicCard,battleField.contactrow);
                        player.AddCardTo(player.board.attackRow,droppedCard);
                        break;
                    case Row.W_distant:
                        GameManager.instance.logicGame.PlayerOnTurn().ActiveCard(logicCard,battleField.distantrow);
                        player.AddCardTo(player.board.distantRow,droppedCard);
                        break;
                    case Row.W_siege:
                        GameManager.instance.logicGame.PlayerOnTurn().ActiveCard(logicCard,battleField.siegerow);
                        player.AddCardTo(player.board.siegeRow,droppedCard);
                        break;     
                }
                // Configura el objeto como hijo del contenedor y ajusta su posición
                droppedCard.transform.SetParent(transform);
                droppedCard.transform.position = transform.position;
               // droppedCard.transform.localScale = cardScale;
                player.board.cardPlayed = true;
                if (dragHandler != null)
                {
                    dragHandler.enabled = false;
                }
                itemsDropped.Add(droppedCard);
                GameManager.instance.WeatherRow.SetActive(false);
                
            }
        }
    }

    
}
