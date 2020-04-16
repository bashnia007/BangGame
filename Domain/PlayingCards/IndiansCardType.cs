using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class IndiansCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Indians;
        protected override bool EqualsCore(CardType other)
        {
            return other is IndiansCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(IndiansCardType).GetHashCode();
        }
    }
}