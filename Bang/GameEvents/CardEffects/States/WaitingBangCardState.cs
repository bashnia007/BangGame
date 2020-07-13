using Bang.Players;
using Bang.PlayingCards;
using System;
using Bang.Game;

namespace Bang.GameEvents.CardEffects.States
{
    internal class WaitingBangCardState : HandlerState
    {
        private readonly Player victim;
        private readonly Player hitter;
        private readonly DefenceStrategy defenceStrategy;
        public WaitingBangCardState(Player victim, Player hitter, HandlerState previousState) : base(previousState)
        {
            this.victim = victim ?? throw new ArgumentNullException(nameof(victim));
            this.hitter = hitter ?? throw new ArgumentNullException(nameof(hitter));
            this.defenceStrategy = new DefenceAgainstIndiansStrategy(hitter);
        }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            bool savedLifePoint = defenceStrategy.Apply(victim, card);
            return new DoneState(this);
        }
    }
}
