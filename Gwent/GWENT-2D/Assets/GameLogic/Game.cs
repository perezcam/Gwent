using System;
namespace engine
{
    public class GameManager
    {
        public GameManager(string nick1, Faction faction1,string nick2, Faction faction2)
        {
          this.player1 = new Player(nick1, faction1);
          this.player2 = new Player (nick2,faction2);
        }

      

        public  Player player1{get;set;}
        public  Player player2{get;set;}

        public static Card Levi = new Card("Levi",0, Type.M, 1, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Eren = new Card("Eren",0, Type.M, 2, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Mikasa = new Card("Mikasa",0, Type.M, 3, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Armin = new Card("Armin",0, Type.M, 4, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Erwin = new Card("Erwin",0, Type.M, 5, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Hange = new Card("Hange",0, Type.M, 6, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Sasha = new Card("Sasha",0, Type.M, 7, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Jean = new Card("Jean",0, Type.M, 8, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Connie = new Card("Connie",0, Type.M, 9, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Historia = new Card("Historia",0, Type.M, 10, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Ymir = new Card("Ymir",0, Type.M, 11, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Marco = new Card("Marco",0, Type.M, 12, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Moblit = new Card("Moblit",0, Type.M, 13, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Nanaba = new Card("Nanaba",0, Type.M, 14, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Gelgar = new Card("Gelgar",0, Type.M, 15, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Petra = new Card("Petra",0, Type.M, 16, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Oluo = new Card("Oluo",10, Type.M, 17, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Eld = new Card("Eld",10, Type.M, 18, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Gunther = new Card("Gunther",0, Type.M, 19, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Dieter = new Card("Dieter",0, Type.M, 20, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Lynne = new Card("Lynne",0, Type.M, 21, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Ilse = new Card("Ilse",0, Type.M, 22, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Oruo = new Card("Oruo",0, Type.M, 23, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card Henning = new Card("Henning",0, Type.M, 24, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
         public static Card BLAMBLAM = new Card("BLAMBLAM",0, Type.M, 25, Faction.Humanity, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card[] humanity = {Levi, Eren, Mikasa ,Armin, Erwin, Hange, Sasha, Jean, Connie, Historia, Ymir, Marco, Moblit, Nanaba, Gelgar, Petra, Oluo, Eld, Gunther, Dieter, Lynne, Ilse, Oruo, Henning,BLAMBLAM};
        
        
        public static Card TitánColosal = new Card("TitánColosal",0, Type.M, 1, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánAcorazado = new Card("TitanAcorazado",0, Type.M, 2, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánBestia = new Card("TitánBestia",0, Type.M, 3, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánFemenino = new Card("TitánFemenino",0, Type.M, 4, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra1 = new Card("TitánMartilloDeGuerra",0, Type.M, 5, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula1 = new Card("TitánMandíbula1",0, Type.M, 6, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánCuadrúpedo = new Card("TitanCuadrupedo",0, Type.M, 7, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánBailarín = new Card("TitanBailarin",0, Type.M, 8, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánEncorvado = new Card("TitánEncorvado",0, Type.M, 9, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMordedor1 = new Card("TitanMordedor",0, Type.M, 10, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánGlotón = new Card("TitanGloton",0, Type.M, 11, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra2 = new Card("TitanMartilloDeGuerra2",0, Type.M, 12, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra3 = new Card("TitanMArtilloDeGuerra3",0, Type.M, 13, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra4 = new Card("TitánMartilloDeGuerra4",0, Type.M, 14, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula2 = new Card("TitánMandíbula2",0, Type.M, 15, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula3 = new Card("TitánMandíbula3",0, Type.M, 16, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánBailarín2 = new Card("TitanBailarin2",0, Type.M, 17, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra5 = new Card("TitanMartillodeGuerra5",0, Type.M, 18, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánEncorvado2 = new Card("TitanEncorvado",0, Type.M, 19, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMordedor2 = new Card("TitanMordedor2",0, Type.M, 20, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánGlotón2 = new Card("TitanGloton2",0, Type.M, 21, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMartilloDeGuerra6 = new Card("TitánMartilloDeGuerra6",0, Type.M, 22, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula4 = new Card("TitanMandibula4",0, Type.M, 23, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula5 = new Card("TitanMandibula5",0, Type.M, 24, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card TitánMandíbula6 = new Card("TitanMandibula6",0, Type.M, 25, Faction.Titans, CardStatus.Ondeck, null!,Card.NullFunction,Row.Null);
        public static Card[] titans = {TitánColosal, TitánAcorazado, TitánBestia, TitánFemenino, TitánMartilloDeGuerra1, TitánMandíbula1, TitánCuadrúpedo, TitánBailarín, TitánEncorvado, TitánMordedor1, TitánGlotón, TitánMartilloDeGuerra2, TitánMartilloDeGuerra3, TitánMartilloDeGuerra4, TitánMandíbula2, TitánMandíbula3, TitánBailarín2, TitánMartilloDeGuerra5, TitánEncorvado2, TitánMordedor2, TitánGlotón2, TitánMartilloDeGuerra6, TitánMandíbula4, TitánMandíbula5,TitánMandíbula6};
      
        
        public static void ConformDeck (Player player)
        {
           
           switch (player.faction)
           {
            case Faction.Humanity: 
                AssignOwner(player,humanity);
                break;
            case Faction.Titans:
                AssignOwner(player,titans);
                break;
           }
           
        }
        public static void AssignOwner (Player player, Card[]faction)
        {
            for (int i = 0; i < 25; i++)
            {
                player.deck[i]= faction[i];
                player.deck[i].owner=player;

            }
        } 
        //Define who player can play
        public static Player TurnPlayer(Player player1, Player player2)
        {
            Player OnTurnPlayer= player2;
            if (player1.OnTurn==false&&player2.OnTurn==false)
            {   
                Random random = new Random();
                int target= random.Next(0,1);
                if (target==0)
                {
                    OnTurnPlayer =player1;
                    OnTurnPlayer.OnTurn=true;
                }
                else
                    OnTurnPlayer.OnTurn=true;
                return OnTurnPlayer;
            }
            else
            {
                if (OnTurnPlayer.OnTurn==true)
                    return player1;
                return OnTurnPlayer;
            }
            

        }
        
        

    }
            
}