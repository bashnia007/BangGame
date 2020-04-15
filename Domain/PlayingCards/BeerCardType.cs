using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class BeerCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Beer;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is BeerCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BeerCardType).GetHashCode();
        }
    }
}