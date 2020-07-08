using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Game;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingMissedCardsAfterGatlingState : HandlerState
    {
        private readonly List<Player> victims;
        private readonly DefenceStrategy defenceStrategy;

        public WaitingMissedCardsAfterGatlingState(List<Player> victims, HandlerState previousState)
            : base(previousState)
        {
            this.victims = victims?? throw new ArgumentNullException(nameof(victims));
            this.defenceStrategy = new DefenceAgainstBangStrategy(gameplay.PlayerTurn, 1);
        }
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard card)
        {
            if (!victims.Contains(victim))
                throw new InvalidOperationException($"Player {victim.Name} is already saved from gatling!");
                
            defenceStrategy.Apply(victim, card);
            victims.Remove(victim);
            
            return victims.Any()? (HandlerState)this : new DoneState(this);
        }
    }
}
