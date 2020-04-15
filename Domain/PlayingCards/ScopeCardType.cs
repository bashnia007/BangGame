using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class ScopeCardType : LongTermFeatureCardType
    {
        public override string Description => CardName.Scope;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is ScopeCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(ScopeCardType).GetHashCode();
        }
        
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
    }
}