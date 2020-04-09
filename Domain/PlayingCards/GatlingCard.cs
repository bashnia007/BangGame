using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class GatlingCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Gatling;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is GatlingCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GatlingCard).GetHashCode();
        }
    }
}