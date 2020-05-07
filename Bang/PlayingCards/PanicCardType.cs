using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class PanicCardType : CardType
    {
        public override string Description => CardName.Panic;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is PanicCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(PanicCardType).GetHashCode();
        }
    }
}