using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class CarabineCard : WeaponCard
    {
        public override string Description => CardName.Carabine;
        protected override bool EqualsCore(PlayingCard other) => other is CarabineCard;

        protected override int GetHashCodeCore() => typeof(CarabineCard).GetHashCode();
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}