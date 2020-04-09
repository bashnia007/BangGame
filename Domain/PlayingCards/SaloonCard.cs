using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class SaloonCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Saloon;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SaloonCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SaloonCard).GetHashCode();
        }
    }
}