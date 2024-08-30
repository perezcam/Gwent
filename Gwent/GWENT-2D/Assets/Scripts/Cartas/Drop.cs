using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

 public enum Row
{
    attackRow,
    distantRow,
    siegeRow,
}
public class Drop : MonoBehaviour, IDropHandler
{
    public Row row;
    public Board board;
    public List<GameObject> itemsDropped = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        // Verifica si ya hay 7 GameObjects en el contenedor
        if (itemsDropped.Count >= 7)
        {
            //Debug.Log("El contenedor ya tiene 7 objetos. No se pueden agregar más.");
            return; //Aqui sale
        }

        GameObject droppedCard = eventData.pointerDrag; 

        if (droppedCard != null)
        {
             Debug.Log(GameManager.instance.player1.playerName);
            //Debug.Log("OnDrop");
            switch (row)
            {
                case Row.attackRow: 
                    board.AddCardTo(board.attackRow,droppedCard);
                    break;
                case Row.distantRow:
                    board.AddCardTo(board.distantRow,droppedCard);
                    break;
                case Row.siegeRow:
                    board.AddCardTo(board.siegeRow,droppedCard);
                    break;
            }
            
        }
            
            // Configura el objeto como hijo del contenedor y ajusta su posición
            droppedCard.transform.SetParent(transform);
            droppedCard.transform.position = transform.position;
        
        
    }
}

