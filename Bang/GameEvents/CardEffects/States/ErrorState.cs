using System;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class ErrorState : HandlerState
    {
        public override bool IsFinalState => true;
        public override bool IsError => true;

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard firstCard, BangGameCard secondCard)
        {
            throw new NotImplementedException();
        }
    }
}