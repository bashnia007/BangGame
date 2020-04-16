using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class MissedCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Missed;
        protected override bool EqualsCore(CardType other)
        {
            return other is MissedCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MissedCardType).GetHashCode();
        }
    }
}