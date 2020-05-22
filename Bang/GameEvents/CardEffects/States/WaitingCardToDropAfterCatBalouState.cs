using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Players;
using Bang.PlayingCards;
using Gameplay = Bang.Game.Gameplay;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToDropAfterCatBalouState : HandlerState
    {
        private Player victim;
        private Game.Gameplay gamePlay;
        private Dictionary<ClosedHandCard, BangGameCard> codeToCardMapping;

        internal WaitingCardToDropAfterCatBalouState(Player victim, Game.Gameplay gameplay, Dictionary<ClosedHandCard, BangGameCard> codeToCardMapping)
        {
            this.victim = victim;
            this.gamePlay = gameplay;
            this.codeToCardMapping = codeToCardMapping;
        }
            
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new System.NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            victim.DropActiveCard(card);
            return new DoneState();
        }

        public override HandlerState ApplyReplyAction(ClosedHandCard cardCode)
        {
            var card = codeToCardMapping[cardCode];
            
            victim.DropCard(card);
            
            return new DoneState();
        }
    }
}