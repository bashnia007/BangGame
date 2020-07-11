using System;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToStealAfterPanicState : HandlerState
    {
        private Player activePlayer;
        private Player victim;

        internal WaitingCardToStealAfterPanicState(Player activePlayer, Player victim, HandlerState previousState) 
            : base(previousState)
        {
            this.activePlayer = activePlayer;
            this.victim = victim;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            activePlayer.DrawPlayerActiveCard(victim, card);
            
            return new DoneState(this);
        }

        public override HandlerState ApplyCardEffect(Player player)
        {
            if (player != victim) throw new InvalidOperationException();
            
            var card = RandomCardChooser.ChooseCard(victim.Hand);
            
            victim.LoseCard(card);
            activePlayer.AddCardToHand(card);

            return new DoneState(this);
        }
    }
}