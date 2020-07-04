using Bang.Characters;
using Bang.Players;
using Bang.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingMissedCardsAfterGatlingState : HandlerState
    {
        protected override CardType ExpectedCard => new MissedCardType();
        private readonly Dictionary<Player, HandlerState> victimStates;
        private readonly Game.Gameplay gameplay;

        public WaitingMissedCardsAfterGatlingState(Dictionary<Player, HandlerState> victimStates, Game.Gameplay gameplay)
        {
            this.victimStates = victimStates;
            this.gameplay = gameplay;
        }
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard card)
        {
            // TODO is it necessary to create another state object? What about victimStates[victim].ApplyReplyAction? 
            var bangState = new WaitingMissedCardAfterBangState(victim, gameplay, 1);
            victimStates[victim] = bangState.ApplyReplyAction(victim, card);
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
