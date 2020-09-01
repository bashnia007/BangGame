using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bang.Exceptions;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class GameoverState : HandlerState
    {
        public override bool IsFinalState => true;

        internal GameoverState(Gameplay gameplay, Team team, List<Player> winners) : base(gameplay)
        {
            Debug.Assert(winners.Any());
            
            SideEffect = new GameOverResponse(team, winners);
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            throw new GameAlreadyOverException();
        }
    }
}