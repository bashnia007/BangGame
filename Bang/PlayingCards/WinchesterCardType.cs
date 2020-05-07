using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class WinchesterCardType : CardType
    {
        public override string Description => CardName.Winchester;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other) => other is WinchesterCardType;
        protected override int GetHashCodeCore() => typeof(WinchesterCardType).GetHashCode();
    }
}