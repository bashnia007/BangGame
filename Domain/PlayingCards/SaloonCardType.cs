using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class SaloonCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Saloon;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is SaloonCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SaloonCardType).GetHashCode();
        }
    }
}