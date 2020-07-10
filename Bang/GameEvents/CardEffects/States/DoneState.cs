using System;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;

namespace Bang.GameEvents.CardEffects.States
{
    internal class DoneState : HandlerState
    {
        public DoneState(Gameplay gameplay) : base(gameplay)
        {
        }

        public DoneState(HandlerState previous) : base(previous)
        {
        }
        
        public override bool IsFinalState => true;
        public override bool IsError => false;

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            var handlerFactory = card.Accept(new GetHandlerVisitor());
            var handler = handlerFactory?.Invoke(gameplay, this, gameplay.PlayerTurn.Character);
            if (handler == null) throw new InvalidOperationException($"Card {card.Description} doesn't have a handler");
            
            return handler.ApplyEffect(player, card);
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard card)
        {
            throw new InvalidOperationException();
        }
    }
}