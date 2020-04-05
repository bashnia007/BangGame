﻿using Domain.PlayingCards.Visitors;

namespace Domain.PlayingCards
{
    public class RemingtonCard : WeaponCard
    {
        public override string Description => CardName.Remington;
        protected override bool EqualsCore(PlayingCard other) => other is RemingtonCard;

        protected override int GetHashCodeCore() => typeof(RemingtonCard).GetHashCode();
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}