﻿using Bang.Players;
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
            return ApplyReplyAction(victim, firstCard);
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard card)
        {
            var bangState = new WaitingBangCardState(victim);
            victimStates[victim] = bangState.ApplyReplyAction(card);
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
