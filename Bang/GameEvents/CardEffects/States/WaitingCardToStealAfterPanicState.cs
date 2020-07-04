using System;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToStealAfterPanicState : HandlerState
    {
        private Player activePlayer;
        private Player victim;

        internal WaitingCardToStealAfterPanicState(Player activePlayer, Player victim)
        {
            this.activePlayer = activePlayer;
            this.victim = victim;
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard card)
        {
            victim.PlayerTablet.RemoveCard(card);
            activePlayer.AddCardToHand(card);
            
            return new DoneState();
        }

        public override HandlerState ApplyReplyAction()
        {
            var card = RandomCardChooser.ChooseCard(victim.Hand);
            
            victim.LoseCard(card);
            activePlayer.AddCardToHand(card);

            return new DoneState();
        }
    }
}