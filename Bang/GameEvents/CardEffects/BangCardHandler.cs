using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class BangCardHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            byte missedRequired = gameplay.PlayerTurn.Character is SlabTheKiller? (byte) 2 : (byte) 1;
            
            var barrelChecker = new BarrelChecker();
            
            if (victim.PlayerTablet.Character.Accept(new HasBarrelByDefaultVisitor()))
            {
                if (barrelChecker.Draw(gameplay, victim.Character))
                    missedRequired--;
            }
            
            if (missedRequired == 0) return new DoneState();
            
            var barrelCard = victim.ActiveCards.FirstOrDefault(c => c == new BarrelCardType());

            if (barrelCard != null)
            {
                if (barrelChecker.Draw(gameplay, victim.Character))
                    missedRequired--;
            }
            
            if (missedRequired == 0) return new DoneState();
            
            // Barrels don't help.
            var response = new DefenceAgainstBang {Player = victim, CardsRequired = missedRequired};
            
            return new WaitingMissedCardAfterBangState(victim, gameplay){SideEffect = response};
        }
    }
}