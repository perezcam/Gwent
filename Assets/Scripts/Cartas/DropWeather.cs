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
     public List<GameObject> itemsDropped;
     public bool ispossible;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();
        Row cardrow = droppedCard.GetComponent<CardDisplay>().row;
        Player cardOwner = droppedCard.GetComponent<CardDisplay>().owner;
        
        if(cardrow == Row.W_attack && !GameManager.instance.weatherows[0])
            ispossible = true;
        else if(cardrow == Row.W_distant && !GameManager.instance.weatherows[1])
        {
             Debug.Log(GameManager.instance.weatherows[1]);
             ispossible = true;
        }
        if(cardrow == Row.W_siege && !GameManager.instance.weatherows[2])
            ispossible = true;
    
        Debug.Log(ispossible);
        if(!ispossible || cardrow != row) 
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
            {  //Revisar si por otro efecto se afecta una carta clima por la logica al agregarlo todo junto
                
                switch (row)
                {   
                    case Row.W_attack:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,1);
                        cardOwner.AddCardTo(cardOwner.board.attackRow,droppedCard);
                        GameManager.instance.weatherows[0] = true;
                        break;
                    case Row.W_distant:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,2);
                        cardOwner.AddCardTo(cardOwner.board.distantRow,droppedCard);
                        GameManager.instance.weatherows[1] = true;
                        break;
                    case Row.W_siege:
                        GameManager.instance.logicGame.PlayerOnTurn().AddtoBattleField(droppedid,3);
                        cardOwner.AddCardTo(cardOwner.board.siegeRow,droppedCard);
                        GameManager.instance.weatherows[2] = true;
                        break;     
                }
                // Configura el objeto como hijo del contenedor y ajusta su posición
                droppedCard.transform.SetParent(transform);
                droppedCard.transform.position = transform.position;
               
               // droppedCard.transform.localScale = cardScale;
                if (dragHandler != null)
                {
                    dragHandler.enabled = false;
                }
                itemsDropped.Add(droppedCard);
                GameManager.instance.WeatherRow.SetActive(false);
                ispossible = false;
                
            }
        }
    }
    
}
