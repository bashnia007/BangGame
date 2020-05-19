using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class SchofieldCardType : CardType
    {
        public override string Description => CardName.Schofield;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is SchofieldCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SchofieldCardType).GetHashCode();
        }
    }
}