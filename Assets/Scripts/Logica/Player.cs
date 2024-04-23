using System.Globalization;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GameLogic
{

    public class Player
    {
        public bool Passed;
        public bool OnTurn;
        public List<Card> hand = new List<Card>();
        public List<Card> deck = new List<Card>();
        public List<int> cardstodelinUI = new List<int>();
        public List<int> W_cardstoshowinUI = new List<int>();
        public BattleField battleField = new BattleField();
        public Dictionary<int, Card> PlayerCardDictionary;
        public string nick { get; set; }
        public int totalforce { get; set; }
       

        public Player(string nick, string faction, Dictionary<int, Card> CardDictionary)
        {
            this.nick = nick;
            PlayerCardDictionary = CardDictionary;
            totalforce = 0;
        }
        public void SetLogicDeck(List<int> UICardID)
        {
            foreach (int ID in UICardID)
            {
                Card card = PlayerCardDictionary[ID].Clone();
                deck.Add(card);
                deck.Last().owner = this;
            }
            PlayerCardDictionary = new Dictionary<int, Card>();
            foreach (Card card in deck)
            {
                PlayerCardDictionary.Add(card.ID,card);
            }
        }
        public void SetLogicHand(List<int> UICardID)
        {
            foreach (int ID in UICardID)
            {
                Card card = PlayerCardDictionary[ID];
                hand.Add(card);
            }
        }
        public void SendCardstoGraveyard()
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (Card card in battleField.CardsonBattleField()[i])
                {
                    cardstodelinUI.Add(card.ID);
                }
            }   
            battleField.contactrow = new List<Card>();
            battleField.distantrow = new List<Card>();
            battleField.siegerow = new List<Card>();
        }
        public void AddtoBattleField(int cardID, int row)
        {
            Card card = PlayerCardDictionary[cardID];
            
            switch (row)
            {
                case 1:
                    ActiveCard(card,battleField.contactrow);
                    AddCardTo(card, battleField.contactrow);
                    break;
                case 2:
                    ActiveCard(card,battleField.distantrow);
                    AddCardTo(card,battleField.distantrow);
                    break;
                case 3:
                    ActiveCard(card,battleField.siegerow);
                    AddCardTo(card,battleField.siegerow);
                    break;
            }
        }
        public void AddCardTo(Card card, List <Card> cardsrow)
        { 
            cardsrow.Add(card);
             try
            {
                hand.Remove(card);
            }
            catch (System.Exception)
            {
                throw;
            }
            //Pone cada carta nueva bajo los efectos de las activas con efectos permanentes
            foreach (Card Card in cardsrow)
            {
                if(Card.cardfunction.function == Functions.IncreasePowerRow && card.ID!=Card.ID)
                {
                    card.powerattack += Card.powerattack;
                    totalforce += Card.powerattack;
                }
            }  
            if(card.owner.battleField.GetWeather(cardsrow)==1 && card.powerattack<=4)
            {
                totalforce -= card.powerattack;
                card.powerattack = 0;
            } 
            else if(card.owner.battleField.GetWeather(cardsrow)==2 && card.cardfunction.function == Functions.IncreasePowerRow) 
            {   // Elimina el efecto provocado por la carta y luego la elimina por ser tipo Increase
                foreach (Card Card in cardsrow)
                {
                    Card.owner.totalforce -= card.powerattack;
                    Card.powerattack -= card.powerattack;
                }
                cardstodelinUI.Add(card.ID); 
            }
        }  

        public void ActiveCard(Card card, List<Card> row)
        {
            //Verifica si hay alguna carta clima en la fila para en caso de existir no activar a la actual
            if ((card.cardfunction.function == Functions.W_ReducePowerOfWeakCards || card.cardfunction.function == Functions.W_ResetCardValues) && card.owner.battleField.GetWeather(row)!=0)
            {
                card.owner.totalforce += card.powerattack;
                return;
            }
            card.cardfunction.function(card,row);
        }
        
        public Player GetEnemy()
        {
            if (this==LogicGameManager.instance.player1)
            return LogicGameManager.instance.player2;
            else
            return LogicGameManager.instance.player1;
        }
        public List<Card> GetRow(int row)
       {  
          switch (row)
          {
                case 1:
                    return battleField.contactrow;
                case 2:
                    return battleField.distantrow;
                case 3:
                    return battleField.siegerow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(row), "Invalid row specified"); 
           }
        }
    }
}