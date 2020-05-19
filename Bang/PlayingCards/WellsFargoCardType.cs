using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class WellsFargoCardType : CardType
    {
        public override string Description => CardName.WellsFargo;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is WellsFargoCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WellsFargoCardType).GetHashCode();
        }
    }
}
