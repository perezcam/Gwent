using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
namespace engine
{
    public class SilverCard:Card
    {
     
        public SilverCard (int powerattack, Type type,int cardId,Faction faction,CardStatus cardstatus)
            :base(powerattack, type, cardId,faction,cardstatus)
        {
            
        }
        
        // Increase the power of cards in a row
        private void IncreasePowerRow (int cardId, Type type, int powerattack)
        {
            throw new NotImplementedException();
        }

        // Remove the card with the most power from the row where it is placed
        private void DelCard ( int CardinRow)
        {
            throw new NotImplementedException();
        }

        // Allows you to draw the selected card
        private void StealCard(int SelectdCard)
        {
            //Si la carta robada ya ha sido sel. un turno anterior no
            //permite ser robada.
            throw new NotImplementedException();
        }

        // Multiplies the attack power of cards by the number of cards
        //on the board
        private void AttackMultiplier(int allcards, int powerattack)
        {
            throw new NotImplementedException();
        }
        // Calculates the avarage of the rival cards and equals it to the 
        //power of the own cards
        private void AvaragedPower(int allcards)
        {
            throw new NotImplementedException();
        }


        




    }
}