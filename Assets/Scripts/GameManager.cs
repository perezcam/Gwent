using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using GameLogic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;


public class GameManager: MonoBehaviour
{
    public GameObject WeatherRow;
    public TextMeshProUGUI player1name;
    public TextMeshProUGUI player2name;
    public Player player1;
    public Player player2;
    public DeckManager deckManager;
    public PanelFader blurPanel;
    public int currenTurn;
    public int currentRound;
    public Player playerOnTurn;
    public LogicGameManager logicGame;
    public static GameManager instance;
    public void Awake()
    {
        WeatherRow.SetActive(false);
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (instance != this) 
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        currenTurn = 0;
        currentRound = 0;
        blurPanel.gameObject.SetActive(false);
        CardLists cardLists = new CardLists();
        deckManager = new DeckManager(cardLists);
        logicGame = new LogicGameManager(SetGameScene.instance.name1, SetGameScene.instance.faction1,SetGameScene.instance.name2, SetGameScene.instance.faction2);
        logicGame.currenTurn=0;
        InitializeGame();
    }

    private void InitializeGame()
    {
        StartCoroutine(InitializePlayers());
    }

    private IEnumerator InitializePlayers()
    {
        player1.Initialize(SetGameScene.instance.name1, SetGameScene.instance.faction1);
        deckManager.InitializeDeck(player1);
        player1.Completehand(10);  
        yield return new WaitUntil(() => player1.hand.Count >= 10);
        player1name.text= player1.playername;

        player2.Initialize(SetGameScene.instance.name2, SetGameScene.instance.faction2);
        deckManager.InitializeDeck(player2);
        player2.Completehand(10); 
        yield return new WaitUntil(() => player2.hand.Count >= 10);
        player2name.text=player2.playername;


        playerOnTurn=player1;
        InitialChange(player1);
        logicGame.player1.SetLogicDeck(CardDatatoInt(player1.deck));
        logicGame.player1.SetLogicHand(GameObjecttoInt(player1.hand));
   }
    public void InitialChange(Player playeronTurn)
    {
        blurPanel.gameObject.SetActive(true);
        deckManager.InitialCardChange(playeronTurn,blurPanel);
        playerOnTurn.board.cardPlayed=false;
    }

   public void PassTurn()
   {
        currenTurn ++;
        logicGame.currenTurn = currenTurn;
    
        //Desactiva la posibilidad de hacer Drag del jugador que paso turno
        playerOnTurn.board.cardPlayed=true;
        
        //Setea el nuevo jugador del turno
        playerOnTurn = (currenTurn%2==0)? player1 : player2;
        
        //permite que la logica sepa que jugador esta en su turno
        SetLogicTurn();
        
        //Hace el cambio de dos cartas inicial
        if(currentRound==0 && currenTurn==1)
        {
            InitialChange(player2);
            logicGame.player2.SetLogicDeck(CardDatatoInt(player2.deck));
            logicGame.player2.SetLogicHand(GameObjecttoInt(player2.hand));
        }
        //Permite que el jugador actual juegue una carta     
        playerOnTurn.board.cardPlayed=false;
        Debug.Log("Turno pasado!");   
   }
    public void PassRound()
    {
        if (currentRound >= 2 && (player1.rwin != player2.rwin))
        {
            Player winner = player1.rwin>player2.rwin ? player1 : player2;
            Debug.Log("El ganador es " + winner.name );
        }
        else if(player1.passed && player2.passed)
        {
            playerOnTurn.board.cardPlayed = true;
            currenTurn = 0;
            currentRound ++;
            playerOnTurn = logicGame.player1.totalforce>= logicGame.player2.totalforce ? player1: player2;
            playerOnTurn.board.cardPlayed = false;
            playerOnTurn.rwin ++;
            while(player1.hand.Count()<=10)
            {
                player1.Completehand(2);
            }

            while(player2.hand.Count()<=10)
            {
                player2.Completehand(2);
            }
            player1.Completehand(2);
        }
        else
        {
            playerOnTurn.passed=true;
            Debug.Log("Ronda pasada!");
        }

    }
    
    public static List<int> CardDatatoInt (List<CardData> list)
    {
        List<int> CardsID = new List<int>(); 
        foreach (CardData card in list)
        {
           
            CardsID.Add(card.ID);
        }
        return CardsID;
    } 
     public static List<int> GameObjecttoInt (List<GameObject> list)
    {
        List<int> CardsID = new List<int>(); 
        foreach (GameObject card in list)
        {
            CardsID.Add(card.GetComponent<CardDisplay>().ID);
        }
        return CardsID;
    } 
    public void SetLogicTurn()
    {
        if (playerOnTurn == player1)
        logicGame.player1.OnTurn=true;
        else
        logicGame.player2.OnTurn=false;
    }

       
   
}




   

