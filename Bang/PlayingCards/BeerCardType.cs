using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class BeerCardType : CardType
    {
        public override string Description => CardName.Beer;
        
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        
        protected override bool EqualsCore(CardType other)
        {
            return other is BeerCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BeerCardType).GetHashCode();
        }
    }
}