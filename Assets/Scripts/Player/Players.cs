using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;
using Unity.VisualScripting;
using System.Linq;
using System.Collections;
using TMPro;
using GameLogic;
using System;
using UnityEngine.Animations;

public class Player:MonoBehaviour
{
    public string playername;
    public List<CardData> deck;
    public List<GameObject> CardInstances;
    public List<GameObject> hand;
    public CardData leader; 
    public TextMeshProUGUI points; 
    public string faction;
    public Board board;
    public CardMover cardMover2D;
    public Dictionary<int, GameObject> CardDictionary;
    public bool passed;
    public int rwin;


    // Método para inicializar el jugador.
    public void Awake()
    {
        points.text= "0";
        deck = new List<CardData>();
        hand = new List<GameObject>();
        CardInstances= new List<GameObject>();
        CardDictionary = new Dictionary<int, GameObject>();
        passed=false;
    }
    public void Initialize(string playername, string faction)
    {
        this.playername = playername;
        this.faction = faction;
    }

    public void Completehand(int numberofcards)
    {
        StartCoroutine(MoveCardsSequentially(CardInstances,numberofcards));
    }
    private IEnumerator MoveCardsSequentially(List<GameObject> cards,int value)
    {
        while(hand.Count<value)
        {
            cardMover2D.MoveCardToHand(cards[0]);
            while (Vector3.Distance(cards[0].transform.position, board.handcontainer.transform.position) > 0.01f)
            {
                // Espera a que la carta actual llegue a su destino antes de comenzar con la siguiente
                yield return null; 
            }
            hand.Add(cards[0]);
            cards.RemoveAt(0);
        }  
    }
    public void AddCardTo(List<GameObject> row,GameObject card)
    { 
        row.Add(card);
        hand.Remove(card);
        board.cardPlayed = true;
        UpdateCardsinGame();
        UpdateCardsPower(row); 
        UpdateCardWeather();
        points.text = GameManager.instance.logicGame.PlayerOnTurn().totalforce.ToString();
        GameManager.instance.logicGame.player1.cardstodelinUI = new List<int>();
        GameManager.instance.logicGame.player2.cardstodelinUI = new List<int>();

    }

    //Actualiza el card.attackvalue de cada carta en caso de que haya sido afectado por un efecto
    public void UpdateCardsPower(List<GameObject> UIrow)
    {
        foreach (GameObject Card in UIrow)
        {
            int CardID = (Card.GetComponent<CardDisplay>()).ID;
            GameLogic.Card logicard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[CardID];
            int UIattackvalue= int.Parse((Card.GetComponent<CardDisplay>()).attackvalue.text);
            int attackvalue = logicard.powerattack;
            if(UIattackvalue!=attackvalue)
            {
                (Card.GetComponent<CardDisplay>()).attackvalue.text = attackvalue.ToString();
            }

        } 
    }
    //Actualiza las cartas en la interfaz del juego a partir de los cambios que ocurran en la logica
    public void UpdateCardsinGame()
    { 
       Player enemy = (GameManager.instance.player1==this)? GameManager.instance.player2: GameManager.instance.player1; 
       foreach (int ID in GameManager.instance.logicGame.PlayerOnTurn().cardstodelinUI)
       {
            //Esto hace el efecto robar una carta
            if (ID==2024)
            {
                Completehand(hand.Count+1);
            }
            else
            {
                try
                {
                    Destroy(CardDictionary[ID]);
                }
                catch (Exception)
                {
                    throw;
                }
            }                
       } 

        foreach (int ID in GameManager.instance.logicGame.PlayerOnTurn().GetEnemy().cardstodelinUI)
       {
              if (ID==2024)
            {
                Completehand(hand.Count+1);
            }
            Debug.Log(ID);
            try
            {
                //Quitar destroy y poner funcion de mover al cementerio cuando este
                Destroy(enemy.CardDictionary[ID]);
            }
            catch (Exception)
            {
                throw;
            }   
       } 
    }
    
    
    // Si una carta te unidad puede cambiar un clima esta funcion coloca un clon de la carta en la fila de cartas climas para evitar q se pueda colocar otro
    //clima en esa fila mientras ella este
    public void UpdateCardWeather()
    {
        foreach (int ID in GameManager.instance.logicGame.PlayerOnTurn().W_cardstoshowinUI)
        {
            try
            {
                InstantiateW_Card(CardDictionary[ID]);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public void InstantiateW_Card(GameObject card)
    {
        GameObject WeatherRow =  GameManager.instance.WeatherRow;
        Transform parent;
        switch (card.GetComponent<CardDisplay>().row)
        {
            case Row.attackRow:
                parent = WeatherRow.GetComponentAtIndex(0).transform;
                break;
            case Row.distantRow:
                parent = WeatherRow.GetComponentAtIndex(1).transform;
                break;
            case Row.siegeRow:
                parent = WeatherRow.GetComponentAtIndex(2).transform;
                break;
            default:
                parent = WeatherRow.transform;
                break;
        }
        
        card.transform.SetParent(parent);
        card.transform.position = transform.position;
        // droppedCard.transform.localScale = cardScale;
    }


    
    
    
    
    
    
    //revisar si funciona precupacion: referencia   
    // public GameLogic.Card[] ConvertUIrowtoRow(List<GameObject> UIrowlist)
    // {
    //     Row UIrow = UIrowlist.ElementAt(0).GetComponentInParent<Row>();
    //     GameLogic.Card[] logicrow = null;
    //     switch (UIrow)
    //     {
    //         case Row.attackRow:
    //             logicrow = GameManager.instance.logicGame.PlayerOnTurn().battleField.contactrow;
    //         break;
    //         case Row.distantRow:
    //             logicrow = GameManager.instance.logicGame.PlayerOnTurn().battleField.distantrow;
    //         break;
    //         case Row.siegeRow:
    //             logicrow = GameManager.instance.logicGame.PlayerOnTurn().battleField.siegerow;
    //         break;
    //         case Row.Leader:
    //            logicrow = GameManager.instance.logicGame.PlayerOnTurn().battleField.leader;
    //         break;
    //         case Row.Wheather:
    //            logicrow = GameManager.instance.logicGame.PlayerOnTurn().battleField.weatherow;
    //         break;
    //     }
    //     return logicrow;
    // }
}

