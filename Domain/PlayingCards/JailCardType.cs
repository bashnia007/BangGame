using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class JailCardType : LongTermFeatureCardType
    {
        public override string Description => CardName.Jail;
        protected override bool EqualsCore(CardType other)
        {
            return other is JailCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JailCardType).GetHashCode();
        }
        
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
    }
}