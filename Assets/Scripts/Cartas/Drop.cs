using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.TextCore.LowLevel;
public enum Row
{
    attackRow,IattackRow,distantRow,IdistantRow,siegeRow,IsiegeRow,Null,Leader,W_attack,W_distant,W_siege,
}
public class Drop : MonoBehaviour, IDropHandler
{
    public Row row;
    public Player player;
    public List<GameObject> itemsDropped = new List<GameObject>();
    private Vector2 cardScale = new Vector2(0.0244f,0.0255f);

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag; 
        DragHandler dragHandler = droppedCard.GetComponent<DragHandler>();
        Row cardrow = droppedCard.GetComponent<CardDisplay>().row;
        Player cardOwner = droppedCard.GetComponent<CardDisplay>().owner;
      
        if(cardOwner!= player ||cardrow!=row||((cardrow==Row.IattackRow||cardrow==Row.IdistantRow||cardrow==Row.IsiegeRow) && itemsDropped.Count==1)||itemsDropped.Count >= 7) 
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

