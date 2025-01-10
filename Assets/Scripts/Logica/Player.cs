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
        public List<int> cardstoShuffleinUI = new List<int>();
        public BattleField battleField = new BattleField();
        public Dictionary<int, Card> PlayerCardDictionary;
        public string nick { get; set; }
        public int totalforce { get; set; }
        public Context context = new Context();
        public Player enemy;
        public List<Card> board{
            get
            {
                List<Card> board = battleField.AllCard();
                board.AddRange(enemy.battleField.AllCard());
                return board;
            }
            private set{}
        }
        public List<Card> field{
            get
            {
                return battleField.AllCard();
            }
            private set{}
        }
        public Player(string nick, string faction, Dictionary<int, Card> CardDictionary)
        {
            this.nick = nick;
            PlayerCardDictionary = CardDictionary;
            totalforce = 0;
        }
        public void SetContext()
        {
            context.hand = hand;
            context.otherhand = enemy.hand;
            context.deck = deck;
            context.otherdeck = enemy.deck;
            context.graveyard = battleField.graveyard;
            context.othergraveyard = enemy.battleField.graveyard;
            context.board = board;
            context.field = field;
            context.otherfield = enemy.field;
        }
        public void SetLogicDeck(List<int> UICardID)
        {
            foreach (int ID in UICardID)
            {
                Card card = PlayerCardDictionary[ID].Clone();
                deck.Add(card);
                deck.Last().Owner = this;
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
                deck.Remove(card);
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
            try
            {
                hand.Remove(card);
            }
            catch (System.Exception)
            {
                throw;
            }
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
        public void AddCardTo(Card card, List <Card> cardsrow, bool top = false)
        { 
            if(top)
                cardsrow.Add(card);
            else
                cardsrow.Prepend(card);
            //Pone cada carta nueva bajo los efectos de las activas con efectos permanentes
            //la comprobacion type != 2 es para asegurar que la carta no es carta heroe
            foreach (Card Card in cardsrow)
            {
                if(Card.cardfunction.function == Functions.IncreasePowerRow && card.ID!=Card.ID && card.type!=2)
                {
                    card.Power += Card.Power;
                    totalforce += Card.Power;
                }
            }  
            if(card.Owner.battleField.GetWeather(cardsrow)==1 && card.Power<=4 && card.type!=2)
            {
                totalforce -= card.Power;
                card.Power = 0;
            } 
            else if(card.Owner.battleField.GetWeather(cardsrow)==2 && card.cardfunction.function == Functions.IncreasePowerRow && card.type!=2) 
            {   // Elimina el efecto provocado por la carta y luego la elimina por ser tipo Increase
                foreach (Card Card in cardsrow)
                {
                    Card.Owner.totalforce -= card.Power;
                    Card.Power -= card.Power;
                }
                cardstodelinUI.Add(card.ID); 
            }
        }  

        public void ActiveCard(Card card, List<Card> row)
        {
            //Verifica si hay alguna carta clima en la fila para en caso de existir no activar a la actual
            if ((card.cardfunction.function == Functions.W_ReducePowerOfWeakCards || card.cardfunction.function == Functions.W_ResetCardValues) && card.Owner.battleField.GetWeather(row)!=0)
            {
                card.Owner.totalforce += card.Power;
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
                case 4:
                    return battleField.graveyard;
                case 5:
                    return deck;
                case 0:
                    return hand;
                case 7:
                    return battleField.contactrow;
                case 8:
                    return battleField.distantrow;
                case 9:
                    return battleField.siegerow;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(row), "Invalid row specified"); 
           }
        }
    }
}