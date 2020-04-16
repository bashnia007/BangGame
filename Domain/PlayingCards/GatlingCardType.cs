using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class GatlingCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Gatling;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is GatlingCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GatlingCardType).GetHashCode();
        }
    }
}