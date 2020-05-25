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
        private readonly Game.Gameplay gameplay;

        private Player currentVictim;

        public WaitingBangCardsAfterIndiansState(Dictionary<Player, HandlerState> victimStates, Game.Gameplay gameplay)
        {
            this.victimStates = victimStates;
            this.gameplay = gameplay;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard firstCard, BangGameCard secondCard)
        {
            currentVictim = victim;
            return ApplyReplyAction(firstCard);
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            var bangState = new WaitingBangCardState(currentVictim);
            victimStates[currentVictim] = bangState.ApplyReplyAction(card);
            return UpdateStatus();
        }

        private HandlerState UpdateStatus()
        {
            return victimStates.All(state => state.Value is DoneState)
                ? new DoneState()
                : (HandlerState)this;
        }
    }
}
