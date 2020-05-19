using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class VolcanicCardType : CardType
    {
        public override string Description => CardName.Volcanic;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other) => other is VolcanicCardType;

        protected override int GetHashCodeCore() => typeof(VolcanicCardType).GetHashCode();
    }
}