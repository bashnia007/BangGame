using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class RemingtonCardType : CardType
    {
        public override string Description => CardName.Remington;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other) => other is RemingtonCardType;

        protected override int GetHashCodeCore() => typeof(RemingtonCardType).GetHashCode();
    }
}