using System;
using System.Collections.Generic;

namespace GameLogic
{ 
    public class LogicGameManager
    {
        //Humanity Deck
        public static Card Armin = new Card("Armin Arlet", 8, 1, null, Functions.AvaragedPower, 2, 0);
        public static Card AtaqueNocturno = new Card("Ataque Nocturno", 0, 2, null, Functions.W_ReducePowerOfWeakCards, 1, 1);
        public static Card EstrategiaErwin = new Card("Estrategia de Erwin", 3, 3, null, Functions.IncreasePowerRow, 3, 1);
        public static Card Connie = new Card("Connie Springer", 4, 4, null, Functions.NullFunction, 1, 0);
        public static Card EM3D = new Card("Equipamiento de Maniobra Tridimensional", 2, 5, null, Functions.IncreasePowerRow, 3, 1);
        public static Card Eren = new Card("Eren Yeager", 11, 6, null, Functions.DelPowerfullCard, 1, 0);
        public static Card Erwin = new Card("Erwin Smith", 6, 7, null, Functions.DelWeakestCard, 1, 0);
        public static Card EstrategiaLevi = new Card("Estrategia de Levi", 4, 8, null, Functions.CleanRow, 1, 0);
        public static Card Flegel = new Card("Flegel Reeves", 3, 9, null, Functions.RemoveWeatherCard, 3, 0);
        public static Card Gabi = new Card("Gabi Braun", 5, 10, null, Functions.IncreasePowerRow, 2, 0);
        public static Card Hange = new Card("Hange Zoë", 5, 11, null, Functions.NullFunction, 0, 3);
        public static Card Hannes = new Card("Hannes", 4, 12, null, Functions.RemoveWeatherCard, 1, 0);
        public static Card Historia = new Card("Historia Reiss", 5, 13, null, Functions.StealCard, 3, 0);
        public static Card Jean = new Card("Jean Kirstein", 6, 14, null, Functions.AttackMultiplier, 1, 0);
        public static Card Levi = new Card("Levi Ackerman", 9, 15, null, Functions.CleanRow, 1, 0);
        public static Card Magaly = new Card("Magaly", 4, 16, null, Functions.W_ResetCardValues, 2, 0);
        public static Card ManiobraDistraccion = new Card("Maniobra de Distracción", 0, 17, null, Functions.W_ResetCardValues, 2, 1);
        public static Card Mikasa = new Card("Mikasa Ackerman", 8, 18, null, Functions.AvaragedPower, 1, 0);
        public static Card Nanaba = new Card("Nanaba", 4, 19, null, Functions.DelWeakestCard, 2, 0);
        public static Card Petra = new Card("Petra Ral", 4, 20, null, Functions.StealCard, 2, 0);
        public static Card RefuerzosTrost = new Card("Refuerzos de Trost", 1, 21, null, Functions.IncreasePowerRow, 2, 1);
        public static Card Sasha = new Card("Sasha Blouse", 6, 22, null, Functions.DelPowerfullCard, 2, 0);
        public static Card Takami = new Card("Takami", 3, 23, null, Functions.W_ReducePowerOfWeakCards, 3, 0);
        public static Card Ymir = new Card("Ymir", 5, 24, null, Functions.IncreasePowerRow, 1, 0);
        public static Card Marco = new Card("Marco Bott", 0, 25, null, Functions.NullFunction, 3, 4);

        //Titans Deck
        public static Card TitanBestia = new Card("Titán Bestia", 0, 26, null, Functions.NullFunction, 0, 3);
        public static Card TitanFundador = new Card("Titán Fundador", 2, 27, null, Functions.IncreasePowerRow, 1, 0);
        public static Card TitanAcorazado = new Card("Titán Acorazado", 5, 28, null, Functions.DelWeakestCard, 1, 0);
        public static Card TitanHembra = new Card("Titán Hembra",6, 29, null, Functions.DelPowerfullCard, 2, 0);
        public static Card TitanColosal = new Card("Titán Colosal", 10, 30, null, Functions.CleanRow, 3, 2);
        public static Card TitanSantaClaus = new Card("Titán Santa Claus", 0, 31, null, Functions.W_ResetCardValues, 2, 1);
        public static Card TitanDentada = new Card("Titán Dentada", 5, 32, null, Functions.NullFunction, 1, 0);
        public static Card TitanGloton = new Card("Titán Glotón", 4, 33, null, Functions.DelPowerfullCard, 1, 0);
        public static Card TitanObervador = new Card("Titán Observador", 3, 34, null, Functions.NullFunction, 2, 0);
        public static Card TitanSonriente = new Card("Titán Sonriente", 5, 35, null, Functions.NullFunction, 1, 0);
        public static Card TitanSaltarin = new Card("Titán Saltarín", 0, 36, null, Functions.Decoy, 5, 4);
        public static Card TitanAgraciado = new Card("Titán Agraciado", 4, 37, null, Functions.NullFunction, 2, 0);
        public static Card TitanCarguero = new Card("Titán Carguero", 6, 38, null, Functions.AvaragedPower, 3, 0);
        public static Card TitanMandibula = new Card("Titán Mandíbula", 6, 39, null, Functions.DelPowerfullCard, 1, 0);
        public static Card TitanMartillodeGuerra = new Card("Titán Martillo de Guerra", 7, 40, null, Functions.RemoveWeatherCard, 3, 0);
        public static Card TitanFalco = new Card("Titán Falco", 4, 41, null, Functions.RemoveWeatherCard, 1, 0);
        public static Card TitanDesCarado = new Card("Titán Descarado", 4, 42, null, Functions.NullFunction, 1, 0);
        public static Card TitanParlante = new Card("Titán Parlante", 3, 43, null, Functions.AttackMultiplier, 2, 0);
        public static Card TitanTatin = new Card("Titán Tatin", 2, 44, null, Functions.IncreasePowerRow, 1, 0);
        public static Card CalorSofocante = new Card("Calor Sofocante", 0, 45, null, Functions.W_ReducePowerOfWeakCards,1, 1);
        public static Card Caminos = new Card("Caminos", 2, 46, null, Functions.IncreasePowerRow, 3, 1);
        public static Card ElDespertar = new Card("El Despertar", 0, 47, null, Functions.W_ResetCardValues, 3, 1);
        public static Card SuerodeTitan = new Card("Suero de Titán", 2, 48, null, Functions.IncreasePowerRow, 2, 1);
        public static Card FuriaTitan = new Card("Furia de Titán", 3, 49, null, Functions.IncreasePowerRow, 1, 1);
        public static Card LLuviadeRocas = new Card("Lluvia de Rocas", 0, 50, null, Functions.W_ResetCardValues, 3, 1);
        public static Card SuenodeFundador = new Card("Sueño de Fundador", 0, 51, null, Functions.W_ReducePowerOfWeakCards, 2, 1);
        public static Card RugidodelTitanBestia = new Card("Rugido del Titán Bestia", 2, 52, null, Functions.RemoveWeatherCard, 2, 0);

        public static Card[] CardDataBase = 
        {
            Armin, AtaqueNocturno, EstrategiaErwin, Connie, EM3D, Eren, Erwin, EstrategiaLevi, Flegel, Gabi, Hange, Hannes, Historia, Jean, Levi, Magaly, ManiobraDistraccion, Mikasa, Nanaba, Petra, RefuerzosTrost, Sasha, Takami, Ymir, Marco,
            TitanBestia, TitanFundador, TitanAcorazado, TitanHembra, TitanColosal, TitanSantaClaus, TitanDentada, TitanGloton, TitanObervador, TitanSonriente, TitanSaltarin, TitanAgraciado, TitanCarguero, TitanMandibula, TitanMartillodeGuerra, TitanFalco, TitanDesCarado, TitanParlante, TitanTatin, CalorSofocante, Caminos, ElDespertar, SuerodeTitan, FuriaTitan, LLuviadeRocas, SuenodeFundador, RugidodelTitanBestia
        };
        public static LogicGameManager instance;
        public int currenTurn = 0;
        public static Dictionary<int, Card> CardDictionary = new Dictionary<int, Card>();
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public LogicGameManager(string nick1, string faction1, string nick2, string faction2)
        {
            InitializeCardDictionary();
            player1 = new Player(nick1, faction1, CardDictionary);
            player2 = new Player(nick2, faction2, CardDictionary);
            instance = this;
        }
        private static void InitializeCardDictionary()
        {
            foreach (Card card in CardDataBase)
            {
                if (!CardDictionary.ContainsKey(card.ID))
                {
                    CardDictionary.Add(card.ID, card);
                }
            }   
        }
 
        public  Player PlayerOnTurn()
        {
            if(currenTurn%2==0)
            {
                player1.OnTurn = true;
                player2.OnTurn = false;
                return player1;
            }
            player2.OnTurn=true;
            player1.OnTurn=false;
            return player2;
        }
    }
}

