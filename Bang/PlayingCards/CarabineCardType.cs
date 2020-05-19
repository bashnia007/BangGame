using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class CarabineCardType : CardType
    {
        public override string Description => CardName.Carabine;
        
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        protected override bool EqualsCore(CardType other) => other is CarabineCardType;

        protected override int GetHashCodeCore() => typeof(CarabineCardType).GetHashCode();
    }
}