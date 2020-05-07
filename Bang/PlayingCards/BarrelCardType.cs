using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class BarrelCardType : CardType
    {
        public override string Description => CardName.Barrel;
        
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        protected override bool EqualsCore(CardType other)
        {
            return other is BarrelCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BarrelCardType).GetHashCode();
        }
    }
}