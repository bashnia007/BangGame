using System;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingCardToStealAfterPanicState : HandlerState
    {
        private static readonly Random random = new Random();

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

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            victim.PlayerTablet.RemoveCard(card);
            activePlayer.AddCardToHand(card);
            
            return new DoneState();
        }
        
        public override HandlerState ApplyReplyAction()
        {
            int number = random.Next(victim.Hand.Count);
            var card = victim.Hand[number];
            
            victim.LoseCard(card);
            activePlayer.AddCardToHand(card);

            return new DoneState();
        }
    }
}