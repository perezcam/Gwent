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
using UnityEngine.UI;

public class Player:MonoBehaviour
{
    public string playername;
    public List<CardData> deck;
    public List<GameObject> CardInstances;
    public List<GameObject> hand;
    public GameObject RoundPoint;
    public Text points; 
    public string faction;
    public Board board;
    public GameObject leader;
    public CardMover cardMover2D;
    public CardMover deleteMover;
    public Dictionary<int, GameObject> CardDictionary;
    public bool passed;
    public int rwin;
    public Player enemy;
    private Vector2 cardScale = new Vector2(0.01303835f,0.01362615f);
    public List<GameObject> cardClones = new List<GameObject>();

    // Método para inicializar el jugador.
    
    public void Awake()
    {
        points.text= "0";
        deck = new List<CardData>();
        hand = new List<GameObject>();
        CardInstances= new List<GameObject>();
        CardDictionary = new Dictionary<int, GameObject>();
        passed=false;
        RoundPoint.transform.GetChild(0).gameObject.SetActive(false);
        RoundPoint.transform.GetChild(1).gameObject.SetActive(false);
        RoundPoint.transform.GetChild(2).gameObject.SetActive(false);
    }
    public void Initialize(string playername, string faction)
    {
        this.playername = playername;
        this.faction = faction;
        
    }
    public void TakeCards()
    {
        foreach (GameObject card in CardInstances)
        {
            if(card.GetComponent<CardDisplay>().row == Row.Leader)
            {
                leader = card;
                Debug.Log(card.GetComponent<CardDisplay>().cardname.text);
                break;
            }
        }
        CardInstances.Remove(leader);
        CardDictionary.Remove(leader.GetComponent<CardDisplay>().ID);
        StartCoroutine(SetLeaderAnimation(leader));
        leader.GetComponent<DragHandler>().enabled = false;
    }
    private IEnumerator SetLeaderAnimation(GameObject leader)
    {
        cardMover2D.MoveCard(leader,board.leaderSlot.transform);
        while (Vector3.Distance(leader.transform.position, board.leaderSlot.transform.position) > 0.01f)
        {
            // Espera a que la carta lider llegue a su destino antes de seguir
            yield return null; 
        }
        Completehand(10);
    }

    public void Completehand(int numberofcards)
    {
        StartCoroutine(MoveCardsSequentially(CardInstances,numberofcards));
    }
    private IEnumerator MoveCardsSequentially(List<GameObject> cards,int value)
    {
        while(hand.Count()<value && hand.Count()<10)
        {
            cardMover2D.MoveCard(cards[0],board.handcontainer.transform);
            while (Vector3.Distance(cards[0].transform.position, board.handcontainer.transform.position) > 0.01f)
            {
                // Espera a que la carta actual llegue a su destino antes de comenzar con la siguiente
                yield return null; 
            }
            hand.Add(cards[0]);
            cards.RemoveAt(0);
        }  
    }
    public void HideOrShowCards()
    {
        foreach (GameObject card in hand)
        {
            if(GameManager.instance.playerOnTurn == this)
            {
                StartCoroutine(RotateCard(card));
                card.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(RotateCard(card));
                card.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
     private IEnumerator RotateCard(GameObject card)
    {
        float halfDuration = 0.25f; 
        float duration = 0.5f; 
        float elapsed = 0; 
    
        Quaternion startRotation = Quaternion.Euler(0, 180,0); 
        Quaternion midRotation = Quaternion.Euler(0, 270, 0); 
        Quaternion endRotation = Quaternion.Euler(0,360, 0);

        while (elapsed < halfDuration)
        {
            card.transform.rotation = Quaternion.Lerp(startRotation, midRotation, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        card.transform.GetChild(0).gameObject.SetActive(true);
        card.transform.GetChild(0).gameObject.SetActive(true);

        while (elapsed < duration)
        {
            card.transform.rotation = Quaternion.Lerp(midRotation, endRotation, (elapsed - halfDuration) / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        card.transform.rotation = endRotation; 
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
        GameManager.instance.logicGame.player1.cardstodelinUI.Clear();
        GameManager.instance.logicGame.player2.cardstodelinUI.Clear();
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
        Debug.Log("afuera " + cardClones.Count());
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
        cardClones.Add(cardclone);
        Debug.Log(cardClones.Count()+ " Instanciando");
        cardclone.transform.SetParent(parent);
        cardclone.transform.localScale = cardScale;
        cardclone.transform.position = parent.position;
        cardclone.GetComponent<DragHandler>().enabled = false;
    }
    public void DeleteCards(List <int> cardid,Player player)
    {  
        List<GameObject> cardstodestroy = new List<GameObject>();
        
        foreach (int ID in cardid)
        {
           cardstodestroy.Add(player.CardDictionary[ID]);
           Debug.Log("en el delete" + player.cardClones.Count());
           foreach(GameObject cardclone in player.cardClones)
           {
                Debug.Log("clon " + cardclone.GetComponent<CardDisplay>().cardname);
                if(ID == cardclone.GetComponent<CardDisplay>().ID)
                {
                    cardstodestroy.Add(cardclone);
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
        }
        
        StartCoroutine(DeleteCardAnimation(cardstodestroy));        
     //   StartCoroutine(DeleteCardAnimation(cardsclonesdestroy));
        cardid.Clear();
    }
    private IEnumerator DeleteCardAnimation(List<GameObject> cardstodelete)
    {
        foreach (GameObject card in cardstodelete)
        {
            deleteMover.MoveCard(card,card.GetComponent<CardDisplay>().owner.board.graveYard.transform);
            while (Vector3.Distance(card.transform.position, card.GetComponent<CardDisplay>().owner.board.graveYard.transform.position) > 0.01f)
            {
                // Espera a que la carta llegue al graveyard antes de seguir
                yield return null; 
            }  
        }
    }
}

