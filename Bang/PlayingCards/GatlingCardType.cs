using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class GatlingCardType : CardType
    {
        public override string Description => CardName.Gatling;
        
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        protected override bool EqualsCore(CardType other)
        {
            return other is GatlingCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GatlingCardType).GetHashCode();
        }
    }
}