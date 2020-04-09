using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class WellsFargoCard : PlayAndDiscardCard
    {
        public override string Description => CardName.WellsFargo;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is WellsFargoCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WellsFargoCard).GetHashCode();
        }
    }
}
