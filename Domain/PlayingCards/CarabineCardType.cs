using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class CarabineCardType : WeaponCardType
    {
        public override string Description => CardName.Carabine;
        protected override bool EqualsCore(CardType other) => other is CarabineCardType;

        protected override int GetHashCodeCore() => typeof(CarabineCardType).GetHashCode();
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
        
    }
}