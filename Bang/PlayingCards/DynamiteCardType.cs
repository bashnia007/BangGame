using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class DynamiteCardType : CardType
    {
        public override string Description => CardName.Dynamite;
        
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        protected override bool EqualsCore(CardType other)
        {
            return other is DynamiteCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DynamiteCardType).GetHashCode();
        }
    }
}