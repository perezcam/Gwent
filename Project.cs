using System.ComponentModel;
using System.Data;

namespace engine
{
    public class Project
    {
        public static void Main(string[] args)
        {
            Card item1 = new Card(2,Type.MRS,6205,Faction.MilitaryPolice,CardStatus.Ondesk);
            Card item2 = new Card(3,Type.MRS,8,Faction.MilitaryPolice,CardStatus.Ondesk);
            Card item3 = new Card(4,Type.R,6204,Faction.ScoutingLegion,CardStatus.Ondesk);
            Player enemy= new Player("Camilo",0,Faction.ScoutingLegion);
            BattleField Enemytable = new BattleField(enemy);
            enemy.InitializeHand();
            Enemytable.AddtoBattleField(item1,Row.Distant);
            Enemytable.AddtoBattleField(item2,Row.Contact);
            Enemytable.AddtoBattleField(item3,Row.Siege);

            
        }

       
            
    }
}