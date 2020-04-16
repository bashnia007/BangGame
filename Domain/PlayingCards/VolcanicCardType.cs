using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class VolcanicCardType : WeaponCardType
    {
        public override string Description => CardName.Volcanic;
        protected override bool EqualsCore(CardType other) => other is VolcanicCardType;

        protected override int GetHashCodeCore() => typeof(VolcanicCardType).GetHashCode();
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
        
    }
}