using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class SaloonCardType : CardType
    {
        public override string Description => CardName.Saloon;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is SaloonCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SaloonCardType).GetHashCode();
        }
    }
}