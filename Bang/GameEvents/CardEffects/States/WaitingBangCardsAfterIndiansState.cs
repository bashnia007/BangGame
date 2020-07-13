using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingBangCardsAfterIndiansState : HandlerState
    {
        private readonly Dictionary<Player, HandlerState> victimStates;
        private readonly Player hitter;

        public WaitingBangCardsAfterIndiansState(Dictionary<Player, HandlerState> victimStates, HandlerState previousState)
            : base(previousState)
        {
            this.victimStates = victimStates;
            this.hitter = gameplay.PlayerTurn;
        }

        public override HandlerState ApplyCardEffect(Player victim, BangGameCard firstCard, BangGameCard secondCard)
        {
            return ApplyCardEffect(victim, firstCard);
        }

        public override HandlerState ApplyCardEffect(Player victim, BangGameCard card)
        {
            var bangState = victimStates[victim];
            victimStates[victim] = bangState.ApplyCardEffect(victim, card);
            return UpdateStatus();
        }

        private HandlerState UpdateStatus()
        {
            return victimStates.All(state => state.Value is DoneState)
                ? new DoneState(this)
                : (HandlerState)this;
        }
    }
}
