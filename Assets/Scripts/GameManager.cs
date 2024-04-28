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
using UnityEngine.Windows.WebCam;
public class GameManager: MonoBehaviour
{
    public GameObject WeatherRow;
    public bool[] weatherows = new bool[3];
    public Text player1name;
    public Text player2name;
    public Player player1;
    public Player player2;
    private Player winner;
    public GameObject GameOver;
    public DeckManager deckManager;
    public PanelFader blurPanel;
    public int currenTurn;
    public int currentRound;
    public Player playerOnTurn;
    public LogicGameManager logicGame;
    public static GameManager instance;
    public Button turnbutton;
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
        turnbutton.gameObject.SetActive(false);
        GameOver.SetActive(false);
        currenTurn = 0;
        currentRound = 0;
        blurPanel.gameObject.SetActive(false);
        CardLists cardLists = new CardLists();
        deckManager = new DeckManager(cardLists);
        logicGame = new LogicGameManager(SetGameScene.instance.gameData.name1, SetGameScene.instance.gameData.faction1,SetGameScene.instance.gameData.name2, SetGameScene.instance.gameData.faction1);
        logicGame.currenTurn=0;
        InitializeGame();
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            WeatherRow.SetActive(true);
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            WeatherRow.SetActive(false);
        } 
    }
    private void InitializeGame()
    {
        StartCoroutine(InitializePlayers());
    }

    private IEnumerator InitializePlayers()
    {

        player1.Initialize(SetGameScene.instance.gameData.name1, SetGameScene.instance.gameData.faction1);
        deckManager.InitializeDeck(player1);
        player1.TakeCards();
        yield return new WaitUntil(() => player1.hand.Count >= 10);
        player1name.text= player1.playername;
        playerOnTurn=player1;

        player2.Initialize(SetGameScene.instance.gameData.name2, SetGameScene.instance.gameData.faction2);
        deckManager.InitializeDeck(player2);
        player2.TakeCards(); 
        yield return new WaitUntil(() => player2.hand.Count >= 10);
        player2name.text=player2.playername;
        foreach(GameObject card in player2.hand)
        {
            card.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        player1.enemy = player2;
        player2.enemy = player1;

        InitialChange(player1);
        logicGame.player1.SetLogicDeck(CardDatatoInt(player1.deck));
        logicGame.player1.SetLogicHand(GameObjecttoInt(player1.hand));
        turnbutton.gameObject.SetActive(true);
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
            //Setea el nuevo jugador del turno
            playerOnTurn = (currenTurn%2==0)? player1 : player2;
            playerOnTurn.passed = false;
            //permite que la logica sepa que jugador esta en su turno
            SetLogicTurn();
            
            //Hace el cambio de dos cartas inicial
            if(currentRound==0 && currenTurn==1)
            { 
                foreach(GameObject card in player2.hand)
                {
                    card.transform.GetChild(1).gameObject.SetActive(true);
                }  
                InitialChange(player2);
                logicGame.player2.SetLogicDeck(CardDatatoInt(player2.deck));
                logicGame.player2.SetLogicHand(GameObjecttoInt(player2.hand));
            }
            player1.HideOrShowCards();
            player2.HideOrShowCards();
            //Permite que el jugador actual juegue una carta     
            playerOnTurn.board.cardPlayed=false;
            Debug.Log("Turno pasado!");   
        }
   }
    public void ChangeRound()
    {  
        if (currentRound <3)
            SetRoundWinner();
        if (player1.rwin == player2.rwin && currentRound > 2)
        {
            if(player1.leader.GetComponent<CardDisplay>().cardname.text== "Hange Zoë" && player2.leader.GetComponent<CardDisplay>().cardname.text!= "Hange Zoë")
            {
                GameOver.SetActive(true);
                GameOver.transform.GetComponent<TextAnimator>().winners[0] = player1.playername;
                GameOver.GetComponent<TextAnimator>().StartTextAnimation();
                return;
            }    
            else if (player2.leader.GetComponent<CardDisplay>().cardname.text== "Hange Zoë" && player1.leader.GetComponent<CardDisplay>().cardname.text!= "Hange Zoë")
            {
                GameOver.SetActive(true);
                GameOver.transform.GetComponent<TextAnimator>().winners[0] = player2.playername;            
                GameOver.GetComponent<TextAnimator>().StartTextAnimation();
                return;
            }   
            else 
            {
                GameOver.SetActive(true);
                GameOver.transform.GetComponent<TextAnimator>().winners[0] = player1.playername;
                GameOver.transform.GetComponent<TextAnimator>().winners[1] = player2.playername;
                GameOver.GetComponent<TextAnimator>().StartTextAnimation();
                return;
            }

        }  

        else if (currentRound>=1 && player1.rwin != player2.rwin)
        {
            winner = player1.rwin > player2.rwin ? player1 : player2;
            GameOver.SetActive(true);
            GameOver.transform.GetComponent<TextAnimator>().winners[0] = winner.playername;
            GameOver.GetComponent<TextAnimator>().StartTextAnimation();
            return;
        }  
        else
        {       
            //Jugar otra ronda
            player1.board.cardPlayed = true;
            player2.board.cardPlayed = true;
            logicGame.player1.totalforce = 0;
            logicGame.player2.totalforce = 0;
            player1.points.text = "0";
            player2.points.text = "0";
            logicGame.player1.SendCardstoGraveyard();
            player1.DeleteCards(logicGame.player1.cardstodelinUI,player1);
            player1.board.ClearUIRows();
            logicGame.player2.SendCardstoGraveyard();
            player2.DeleteCards(logicGame.player2.cardstodelinUI,player2);
            player2.board.ClearUIRows();
            currentRound ++;
            player1.CompleteandRotateHand(player1.leader.GetComponent<CardDisplay>().cardname.text == "T.Bestia" ? player1.hand.Count()+3 : player1.hand.Count()+2);
            logicGame.player1.hand = new List<Card>();
            logicGame.player1.SetLogicHand(GameObjecttoInt(player1.hand));
            player2.CompleteandRotateHand(player2.leader.GetComponent<CardDisplay>().cardname.text == "T.Bestia" ? player2.hand.Count()+3 : player2.hand.Count()+2);
            logicGame.player2.hand = new List<Card>();
            logicGame.player2.SetLogicHand(GameObjecttoInt(player2.hand));
            player1.passed = false;
            player2.passed = false;
            playerOnTurn.board.cardPlayed = false;
        }
    }
    public  void SetRoundWinner()
    {
        if(logicGame.player1.totalforce>logicGame.player2.totalforce)
        {
            player1.rwin ++;
            player1.RoundPoint.transform.GetChild(player1.rwin-1).gameObject.SetActive(true);
            playerOnTurn = player1;
            currenTurn = 0;
            logicGame.currenTurn = 0;
            SetLogicTurn();
            
        }
        else if (logicGame.player1.totalforce==logicGame.player2.totalforce)
        {
            player1.rwin ++;
            player2.rwin ++;
            player1.RoundPoint.transform.GetChild(player1.rwin-1).gameObject.SetActive(true);
            player2.RoundPoint.transform.GetChild(player2.rwin-1).gameObject.SetActive(true);
            playerOnTurn = player1;
            currenTurn = 0;
            logicGame.currenTurn = 0;
            SetLogicTurn();
        }
        else
        {
            player2.rwin ++;
            player2.RoundPoint.transform.GetChild(player2.rwin-1).gameObject.SetActive(true);
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




   

