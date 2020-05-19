using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class JailCardType : CardType
    {
        public override string Description => CardName.Jail;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is JailCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JailCardType).GetHashCode();
        }
    }
}