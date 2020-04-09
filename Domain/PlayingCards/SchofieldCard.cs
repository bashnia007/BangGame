using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class SchofieldCard : WeaponCard
    {
        public override string Description => CardName.Schofield;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SchofieldCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SchofieldCard).GetHashCode();
        }

        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
    }
}