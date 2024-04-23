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
using System.Threading.Tasks;
public class GameManager: MonoBehaviour
{
    public GameObject WeatherRow;
    public TextMeshProUGUI player1name;
    public TextMeshProUGUI player2name;
    public Player player1;
    public Player player2;
    private Player winner;
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
        logicGame = new LogicGameManager(SetGameScene.instance.gameData.name1, SetGameScene.instance.gameData.faction1,SetGameScene.instance.gameData.name2, SetGameScene.instance.gameData.faction1);
        logicGame.currenTurn=0;
        InitializeGame();
    }

    private void InitializeGame()
    {
        StartCoroutine(InitializePlayers());
    }

    private IEnumerator InitializePlayers()
    {
        player1.Initialize(SetGameScene.instance.gameData.name1, SetGameScene.instance.gameData.faction1);
        deckManager.InitializeDeck(player1);
        player1.Completehand(10);  
        yield return new WaitUntil(() => player1.hand.Count >= 10);
        player1name.text= player1.playername;

        player2.Initialize(SetGameScene.instance.gameData.name2, SetGameScene.instance.gameData.faction2);
        deckManager.InitializeDeck(player2);
        player2.Completehand(10); 
        yield return new WaitUntil(() => player2.hand.Count >= 10);
        player2name.text=player2.playername;
        
        player1.enemy = player2;
        player2.enemy = player1;

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
        playerOnTurn.passed = true;
        
        if (player1.passed && !player1.board.cardPlayed && player2.passed && !player2.board.cardPlayed )
            ChangeRound();
        else
        {
            currenTurn ++;
            logicGame.currenTurn = currenTurn;
            Debug.Log(currentRound + "Esta es la ronda actual");
            //Setea el nuevo jugador del turno
            playerOnTurn = (currenTurn%2==0)? player1 : player2;
            playerOnTurn.passed = false;
            
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
   }
    public void ChangeRound()
    {  
        SetRoundWinner();
        if (player1.rwin == player2.rwin && currentRound ==2)
        {
            Debug.Log("empate");            
        }     
        else if (currentRound>=1 && player1.rwin != player2.rwin)
        {
            winner = player1.rwin > player2.rwin ? player1 : player2;
            Debug.Log(winner.playername + "Es el ganador"); 
            blurPanel.gameObject.SetActive(true);
        }  
        else
        { //Jugar otra ronda
                player1.board.cardPlayed = true;
                player2.board.cardPlayed = true;
                logicGame.player1.totalforce = 0;
                logicGame.player2.totalforce = 0;
                player1.points.text = "0";
                player2.points.text = "0";
                logicGame.player1.SendCardstoGraveyard();
                player1.DeleteCards(logicGame.player1.cardstodelinUI,player1);
                logicGame.player2.SendCardstoGraveyard();
                player2.DeleteCards(logicGame.player2.cardstodelinUI,player2);
                currentRound ++;
                player1.Completehand(player1.hand.Count()+2);
                logicGame.player1.hand = new List<Card>();
                logicGame.player1.SetLogicHand(GameObjecttoInt(player1.hand));
                player2.Completehand(player2.hand.Count()+2);
                logicGame.player2.hand = new List<Card>();
                logicGame.player2.SetLogicHand(GameObjecttoInt(player2.hand));
                player1.passed = false;
                player2.passed = false;
                playerOnTurn.board.cardPlayed = false;
        }
        //Falta eliminar el clon al hacer una carta clima desaparecer
        
    }
    public  void SetRoundWinner()
    {
        if(logicGame.player1.totalforce>logicGame.player2.totalforce)
        {
            player1.rwin ++;
            playerOnTurn = player1;
            currenTurn = 0;
            logicGame.currenTurn = 0;
            SetLogicTurn();
            
        }
        else if (logicGame.player1.totalforce==logicGame.player2.totalforce)
        {
            player1.rwin ++;
            player2.rwin ++;
            playerOnTurn = player1;
            currenTurn = 0;
            logicGame.currenTurn = 0;
            SetLogicTurn();
        }
        else
        {
            player2.rwin ++;
            playerOnTurn = player2;
            currenTurn = 1;
            logicGame.currenTurn = 1;
            SetLogicTurn();
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




   

