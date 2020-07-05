using System;
using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class WaitingMissedCardAfterBangState : HandlerState
    {
        public override bool IsFinalState => false;
        public override bool IsError => false;

        private Player victim;
        private Game.Gameplay gameplay;
        private DefenceStrategy defenceStrategy;
        
        public WaitingMissedCardAfterBangState(Player victim, Game.Gameplay gameplay, int requiredCards)
        {
            this.victim = victim;
            this.gameplay = gameplay;

            this.defenceStrategy = new DefenceAgainstBangStrategy(gameplay.PlayerTurn, requiredCards);
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card, Game.Gameplay gameplay)
        {
            throw new InvalidOperationException();
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard card)
        {
            if (victim != player)
                throw new InvalidOperationException();
            
            defenceStrategy.Apply(victim, card);
            return new DoneState();
        }

        public override HandlerState ApplyReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ApplyReplyAction(victim, firstCard);

            if (victim != player)
                throw new InvalidOperationException();

            defenceStrategy.Apply(victim, firstCard, secondCard);
            
            return new DoneState();
        }
    }
}