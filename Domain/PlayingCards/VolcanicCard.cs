﻿using Domain.PlayingCards.Visitors;

namespace Domain.PlayingCards
{
    public class VolcanicCard : WeaponCard
    {
        public override string Description => CardName.Volcanic;
        protected override bool EqualsCore(PlayingCard other) => other is VolcanicCard;

        protected override int GetHashCodeCore() => typeof(VolcanicCard).GetHashCode();
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}