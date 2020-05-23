using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects
{
    internal class GatlingActionHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player attackPlayer, BangGameCard card)
        {
            var victimStatesList = new Dictionary<Player, HandlerState>();

            foreach (var victim in gameplay.Players.Where(p => p.Id != attackPlayer.Id))
            {
                victimStatesList.Add(victim, TryShootPlayer(victim, gameplay));
            }

            return new WaitingMissedCardsAfterGatlingState(victimStatesList, gameplay);
        }

        private HandlerState TryShootPlayer(Player victim, Game.Gameplay gameplay)
        {
            byte missedRequired = gameplay.PlayerTurn.Character is SlabTheKiller ? (byte)2 : (byte)1;

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
            var response = new DefenceAgainstBang { Player = victim, CardsRequired = missedRequired };

            return new WaitingMissedCardAfterBangState(victim, gameplay) { SideEffect = response };
        }
    }
}
