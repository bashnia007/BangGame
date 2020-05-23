using System;
using Bang.Players;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;
using Bang.Game;

namespace Bang.GameEvents.CardEffects.States
{
    public class DoneState : HandlerState
    {
        public override bool IsFinalState => true;
        public override bool IsError => false;

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gamePlay)
        {
            var handler = card.Accept(new GetHandlerVisitor());
            if (handler == null) throw new InvalidOperationException($"Card {card.Description} doesn't have a handler");
            
            return handler.ApplyEffect(gamePlay, player, card);
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new InvalidOperationException();
        }
    }
}