using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class MustangCardType : CardType
    {
        public override string Description => CardName.Mustang;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is MustangCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MustangCardType).GetHashCode();
        }
    }
}