using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingBangCardsAfterIndiansState : HandlerState
    {
        protected override CardType ExpectedCard => new BangCardType();
        private readonly Dictionary<Player, HandlerState> victimStates;
        private readonly Game.Gameplay gameplay;
        private readonly Player hitter;

        public WaitingBangCardsAfterIndiansState(Dictionary<Player, HandlerState> victimStates, Game.Gameplay gameplay)
        {
            this.victimStates = victimStates;
            this.gameplay = gameplay;
            this.hitter = gameplay.PlayerTurn;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard firstCard, BangGameCard secondCard)
        {
            return ApplyReplyAction(victim, firstCard);
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard card)
        {
            var bangState = new WaitingBangCardState(victim, hitter);
            victimStates[victim] = bangState.ApplyReplyAction(victim, card);
            return UpdateStatus();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new NotImplementedException();
        }

        private HandlerState UpdateStatus()
        {
            return victimStates.All(state => state.Value is DoneState)
                ? new DoneState()
                : (HandlerState)this;
        }
    }
}
