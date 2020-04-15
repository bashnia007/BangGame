using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class DuelCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Duel;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is DuelCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DuelCardType).GetHashCode();
        }
    }
}