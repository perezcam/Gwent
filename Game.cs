namespace engine
{
    public class Game
    {
        public Game()
        {

        }
        public static void AddCardTo(Card[] cardsrow, Card card)
       {
            for (int i = 0; i < cardsrow.Length; i++)
            {
                if(cardsrow[i] == null)
                {
                    cardsrow[i] = card;
                    break;
                }
                else if (cardsrow[i] != null && i==cardsrow.Length-1)
                throw new Exception ("No caben mas cartas");
                continue;
            }
       }
    }
}