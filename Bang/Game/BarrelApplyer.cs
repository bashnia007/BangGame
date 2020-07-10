using System.Linq;
using Bang.Characters.Visitors;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game
{
    // TODO create more concrete name
    static class BarrelApplyer
    {
        private static BarrelChecker barrelChecker = new BarrelChecker(); 
        public static void ApplyBarrel(Gameplay gameplay, Player victim, ref int missedRequired)
        {
            if (victim.Character.Accept(new HasBarrelByDefaultVisitor()))
            {
                if (barrelChecker.Draw(gameplay, victim.Character))
                    missedRequired--;
            }
            
            if (missedRequired == 0) return;
            
            var barrelCard = victim.ActiveCards.FirstOrDefault(c => c == new BarrelCardType());

            if (barrelCard != null)
            {
                if (barrelChecker.Draw(gameplay, victim.Character))
                    missedRequired--;
            }
        }
    }
}