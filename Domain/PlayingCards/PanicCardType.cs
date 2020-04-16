using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class PanicCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Panic;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is PanicCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(PanicCardType).GetHashCode();
        }
    }
}