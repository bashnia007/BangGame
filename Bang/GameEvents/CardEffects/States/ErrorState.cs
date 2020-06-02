using System;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects.States
{
    internal class ErrorState : HandlerState
    {
        public override bool IsFinalState => true;
        public override bool IsError => true;
        public override Response SideEffect { get; set; } = new NotAllowedOperation();
        
        // TODO add constructor with side effect parameter

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new NotImplementedException();
        }
    }
}