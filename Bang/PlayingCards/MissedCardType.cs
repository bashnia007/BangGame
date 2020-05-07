using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class MissedCardType : CardType
    {
        public override string Description => CardName.Missed;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is MissedCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MissedCardType).GetHashCode();
        }
    }
}