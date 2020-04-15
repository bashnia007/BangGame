using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class BarrelCardType : LongTermFeatureCardType
    {
        public override string Description => CardName.Barrel;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is BarrelCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BarrelCardType).GetHashCode();
        }

        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
    }
}