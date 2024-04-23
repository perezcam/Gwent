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
    public Player enemy;
    private Vector2 cardScale = new Vector2(0.01303835f,0.01362615f);
    private List<GameObject> cardClones = new List<GameObject>();

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
        while(hand.Count()<value && hand.Count()<10)
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
        card.AddComponent<CardDropHandler>();
        row.Add(card);
        hand.Remove(card);
        board.cardPlayed = true;
        UpdateCardsPower(); 
        UpdateCardWeather();
        UpdateCardsinGame();
        
        points.text = GameManager.instance.logicGame.PlayerOnTurn().totalforce.ToString();
        enemy.points.text = GameManager.instance.logicGame.PlayerOnTurn().GetEnemy().totalforce.ToString();
        
        

    }

    //Actualiza el card.attackvalue de cada carta en caso de que haya sido afectado por un efecto
    public void UpdateCardsPower()
    {
        List<GameObject> playerOnTurnRows = new List<GameObject>();
        playerOnTurnRows.AddRange(board.attackRow);
        playerOnTurnRows.AddRange(board.distantRow);
        playerOnTurnRows.AddRange(board.siegeRow);

        List<GameObject> enemyrows = new List<GameObject>();
        enemyrows.AddRange(enemy.board.attackRow);
        enemyrows.AddRange(enemy.board.distantRow);
        enemyrows.AddRange(enemy.board.siegeRow);

        
        foreach (GameObject Card in playerOnTurnRows)
        {
            int CardID = Card.GetComponent<CardDisplay>().ID;
            GameLogic.Card logicard = GameManager.instance.logicGame.PlayerOnTurn().PlayerCardDictionary[CardID];
            int UIattackvalue= int.Parse((Card.GetComponent<CardDisplay>()).attackvalue.text);
            int attackvalue = logicard.powerattack;
            if(UIattackvalue!=attackvalue)
            {
                Card.GetComponent<CardDisplay>().attackvalue.text = attackvalue.ToString();
            }
        } 
        foreach (GameObject Card in enemyrows)
        {
            int CardID = Card.GetComponent<CardDisplay>().ID;
            GameLogic.Card logicard = GameManager.instance.logicGame.PlayerOnTurn().GetEnemy().PlayerCardDictionary[CardID];
            int UIattackvalue= int.Parse((Card.GetComponent<CardDisplay>()).attackvalue.text);
            int attackvalue = logicard.powerattack;
            if(UIattackvalue!=attackvalue)
            {
                Card.GetComponent<CardDisplay>().attackvalue.text = attackvalue.ToString();
            }
        } 

        playerOnTurnRows = new List<GameObject>();
        enemyrows = new List<GameObject>();
    }
    //Actualiza las cartas en la interfaz del juego a partir de los cambios que ocurran en la logica
    public void UpdateCardsinGame()
    { 
       
        if ( GameManager.instance.logicGame.PlayerOnTurn().cardstodelinUI.Contains(2024))
        {
            Completehand(hand.Count+1);
            GameManager.instance.logicGame.PlayerOnTurn().cardstodelinUI.Remove(2024);
        }
                          
        DeleteCards(GameManager.instance.logicGame.PlayerOnTurn().cardstodelinUI,this);
        DeleteCards(GameManager.instance.logicGame.PlayerOnTurn().GetEnemy().cardstodelinUI,enemy);  
        GameManager.instance.logicGame.player1.cardstodelinUI = new List<int>();
        GameManager.instance.logicGame.player2.cardstodelinUI = new List<int>();
    }
    
    
    // Si una carta de unidad puede cambiar un clima esta funcion coloca un clon de la carta en la fila de cartas climas para evitar q se pueda colocar otro
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
        GameManager.instance.logicGame.player1.W_cardstoshowinUI = new List<int>();
        GameManager.instance.logicGame.player2.W_cardstoshowinUI = new List<int>();
    }

    public void InstantiateW_Card(GameObject card)
    {
        GameObject WeatherRow =  GameManager.instance.WeatherRow;
        Transform parent = WeatherRow.transform;
        switch (card.GetComponent<CardDisplay>().row)
        {
            case Row.attackRow:
                parent = parent.transform.GetChild(0).transform;
                break;
            case Row.distantRow:
                parent = parent.transform.GetChild(1).transform;
                break;
            case Row.siegeRow:
                parent = parent.transform.GetChild(2).transform;;
                break;
        }
        GameObject cardclone = Instantiate(card, parent);
        cardclone.transform.SetParent(parent);
        cardclone.transform.localScale = cardScale;
        cardclone.transform.position = parent.position;
        parent.gameObject.GetComponent<DropWeather>().itemsDropped.Add(cardclone);
        cardClones.Add(cardclone);
    }
    public void DeleteCards(List <int> cardid,Player player)
    {  
        List<GameObject> cardstodestroy = new List<GameObject>();
        List<GameObject> cardsclonesdestroy = new List<GameObject>();
        
        foreach (int ID in cardid)
        {
           cardstodestroy.Add(player.CardDictionary[ID]);
           foreach(GameObject cardclone in cardClones)
           {
                if(ID == cardclone.GetComponent<CardDisplay>().ID)
                {
                    cardsclonesdestroy.Add(cardclone);
                }
           }
        }
        foreach (GameObject card in cardstodestroy)
        {
            if (player.board.attackRow.Contains(card))
                player.board.attackRow.Remove(card);
            else if (player.board.distantRow.Contains(card))
                player.board.distantRow.Remove(card);
            else 
                player.board.siegeRow.Remove(card);
            
            card.transform.SetParent(player.board.graveYard.transform);
            card.transform.position = player.board.graveYard.transform.position;
            card.transform.localScale = cardScale;
            
            card.SetActive(false);
        }
        foreach (GameObject cardclone in cardsclonesdestroy)
        {
            cardclone.SetActive(false);
        }
        cardid.Clear();
    }



}

