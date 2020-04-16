using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class MustangCardType : LongTermFeatureCardType
    {
        public override string Description => CardName.Mustang;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is MustangCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MustangCardType).GetHashCode();
        }
        
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
    }
}