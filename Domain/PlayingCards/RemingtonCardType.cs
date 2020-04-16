using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class RemingtonCardType : WeaponCardType
    {
        public override string Description => CardName.Remington;
        protected override bool EqualsCore(CardType other) => other is RemingtonCardType;

        protected override int GetHashCodeCore() => typeof(RemingtonCardType).GetHashCode();
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
        
    }
}