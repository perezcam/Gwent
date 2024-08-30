using System;

namespace engine
{
    public class Project
    {
         public static GameManager Game= new GameManager("Camilo",Faction.Humanity,"Priscila",Faction.Titans);
        public static void Main(string[] args)
        {      
              Project.StartGame();
        }
          public static void StartGame()
        { 
          GameManager.ConformDeck(Game.player1);
          GameManager.ConformDeck(Game.player2);
          Turn();
          while(Game.player1.Passed||Game.player2.Passed)
          {
             Player player = GameManager.TurnPlayer(Game.player1,Game.player2);
             Turn();

          }
        
        }
        public static void Turn()
        {
            Player player = GameManager.TurnPlayer(Game.player1,Game.player2);
            Console.WriteLine(player.nick + " marca {1} para jugar y {2} para pasar de turno");
            int decision = int.Parse(Console.ReadLine());
            
            if (decision==1)
            {
                player.InitializeHand();
                Console.WriteLine("Esta es tu mano de cartas");
                PrintHand(player);
                Console.WriteLine("Tienes permitido de hacer un cambio inicial de  dos cartas,deseas hacerlo ahora?");
                Console.WriteLine("Presiona {1} para hacerlo y {2} para no hacerlo");
                decision=int.Parse(Console.ReadLine());
                if (decision==1)
                {
                  Console.WriteLine("Selecciona el numero de la primera carta a cambiar");
                  int card1 = int.Parse(Console.ReadLine());
                  Console.WriteLine("Selecciona el numero de la segunda carta a cambiar");
                  int card2 = int.Parse(Console.ReadLine());                
                  
                  player.InitialChange(player.hand[card1],player.hand[card2]);
                  Console.WriteLine("Cambio realizado, este es tu mazo final");
                  PrintHand(player);
                }
                Console.WriteLine("Este es su tablero");
                PrintBoard(player);
                Console.WriteLine("Para agregar una carta al tablero");
                Console.WriteLine("Inserte el numero de la carta");
                decision=int.Parse(Console.ReadLine());
                Console.WriteLine("Inserte el numero de la fila que desea");
                Console.WriteLine(" {0}ContactRow \n {1}DistantRow \n {2}SiegeRow");
                int row=int.Parse(Console.ReadLine());
                switch (row)
                {
                  case 0:
                    player.AddtoBattleField(player.hand[decision],Row.Contact);
                    break;
                  case 1:
                    player.AddtoBattleField(player.hand[decision],Row.Distant);
                    break;
                  case 2:
                    player.AddtoBattleField(player.hand[decision],Row.Siege);
                    break;
                }
                Console.WriteLine("Ya solo puedes pasar de turno");
                Console.WriteLine("Presiona 1 para pasar");
                decision = int.Parse(Console.ReadLine());                
            }  
            else
            {
                player.Passed=true;
            }
        }

        public static void PrintHand(Player player)
        {
          for (int i = 0; i < 10; i++)
                {
                  Console.WriteLine( "["+i+"]"+ player.hand[i].name);
                }
        }

        public static void PrintBoard(Player player)
        {
           PrintRow(player.battleField.contactrow,Row.Contact);

           PrintRow(player.battleField.distantrow,Row.Distant);
           PrintRow(player.battleField.siegerow,Row.Siege);
           Console.WriteLine("<<<<<<<"+player.totalforce+">>>>>>>");
        }  


        public static void PrintRow (Card[]cardsrow,Row row)
        {
            for (int i = 0; i < 9; i++)
            {
                if (cardsrow[i]==null)
                Console.Write("|_0_| ");
                else
                Console.Write("|_1_| ");

            }
            Console.Write(row + " row");
            Console.WriteLine("\n");
          
        }
     }   
}