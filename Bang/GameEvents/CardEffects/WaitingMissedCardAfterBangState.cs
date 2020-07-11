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
        private DefenceAgainstBangStrategy defenceStrategy;
        
        public WaitingMissedCardAfterBangState(Player victim, DefenceAgainstBangStrategy defenceStrategy, HandlerState previousState) 
            : base(previousState)
        {
            this.victim = victim ?? throw new ArgumentNullException(nameof(victim));
            this.defenceStrategy = defenceStrategy ?? throw new ArgumentNullException(nameof(defenceStrategy));
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            if (victim != player)
                throw new InvalidOperationException();
            
            defenceStrategy.Apply(victim, card);
            return new DoneState(this);
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ApplyCardEffect(victim, firstCard);

            if (victim != player)
                throw new InvalidOperationException();

            defenceStrategy.Apply(victim, firstCard, secondCard);
            
            return new DoneState(this);
        }
    }
}