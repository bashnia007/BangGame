using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class BangCardType : CardType
    {
        public override string Description => CardName.Bang;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(CardType other)
        {
            return other is BangCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BangCardType).GetHashCode();
        }
    }
}