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
        private static Random random = new Random();
        private Player victim;
        private Game.Gameplay gamePlay;

        internal WaitingCardToDropAfterCatBalouState(Player victim, Game.Gameplay gameplay)
        {
            this.victim = victim;
            this.gamePlay = gameplay;
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

        public override HandlerState ApplyReplyAction()
        {
            int number = random.Next(victim.Hand.Count);
            var card = victim.Hand[number];
            
            victim.DropCard(card);
            
            return new DoneState();
        }
    }
}