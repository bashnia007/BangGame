using System;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToDropAfterCatBalouState : HandlerState
    {
        private Player victim;

        internal WaitingCardToDropAfterCatBalouState(Player victim, HandlerState previousState) : base(previousState)
        {
            this.victim = victim;
        }
            
        
        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            victim.DiscardActiveCard(card);
            return new DoneState(this);
        }

        public override HandlerState ApplyCardEffect(Player player)
        {
            if (player != victim) throw new InvalidOperationException();
            
            var card = RandomCardChooser.ChooseCard(victim.Hand);
            victim.DropCard(card);
            
            return new DoneState(this);
        }
    }
}