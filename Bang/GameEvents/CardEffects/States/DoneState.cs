using System;
using Bang.Players;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;

namespace Bang.GameEvents.CardEffects.States
{
    internal class DoneState : HandlerState
    {
        public override CardType ExpectedCard => new BangCardType();
        public override bool IsFinalState => true;
        public override bool IsError => false;

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gamePlay)
        {
            var handler = card.Accept(new GetHandlerVisitor(gamePlay.PlayerTurn.Character));
            if (handler == null) throw new InvalidOperationException($"Card {card.Description} doesn't have a handler");
            
            return handler.ApplyEffect(gamePlay, player, card);
        }

        public override HandlerState ApplyReplyAction(BangGameCard card)
        {
            throw new InvalidOperationException();
        }
    }
}