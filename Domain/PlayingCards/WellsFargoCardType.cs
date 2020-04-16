using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class WellsFargoCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.WellsFargo;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is WellsFargoCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WellsFargoCardType).GetHashCode();
        }
    }
}
