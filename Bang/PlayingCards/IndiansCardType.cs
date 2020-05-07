using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class IndiansCardType : CardType
    {
        public override string Description => CardName.Indians;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is IndiansCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(IndiansCardType).GetHashCode();
        }
    }
}