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
        private readonly Dictionary<Player, HandlerState> victimStates;
        private readonly Game.Gameplay gameplay;

        private Player currentVictim;

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
            var bangState = new WaitingMissedCardAfterBangState(currentVictim, gameplay);
            victimStates[currentVictim] = bangState.ApplyReplyAction(card);
            return UpdateStatus();
        }

        public override HandlerState ApplyReplyAction(Player victim, BangGameCard firstCard, BangGameCard secondCard)
        {
            currentVictim = victim;
            var bangState = new WaitingMissedCardAfterBangState(victim, gameplay);
            victimStates[victim] = bangState.ApplyReplyAction(victim, firstCard, secondCard);
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
