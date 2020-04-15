using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class DynamiteCardType : LongTermFeatureCardType
    {
        public override string Description => CardName.Dynamite;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is DynamiteCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DynamiteCardType).GetHashCode();
        }

        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
    }
}