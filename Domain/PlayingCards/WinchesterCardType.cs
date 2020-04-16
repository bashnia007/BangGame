using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class WinchesterCardType : WeaponCardType
    {
        public override string Description => CardName.Winchester;
        protected override bool EqualsCore(CardType other) => other is WinchesterCardType;
        protected override int GetHashCodeCore() => typeof(WinchesterCardType).GetHashCode();
        public override T Accept<T>(ILongTermCardTypeVisitor<T> visitor) => visitor.Visit(this);
        
    }
}