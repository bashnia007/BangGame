using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class DuelCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Duel;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is DuelCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DuelCard).GetHashCode();
        }
    }
}