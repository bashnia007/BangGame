using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class ScopeCardType : CardType
    {
        public override string Description => CardName.Scope;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is ScopeCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(ScopeCardType).GetHashCode();
        }
    }
}