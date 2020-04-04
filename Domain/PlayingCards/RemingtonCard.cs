﻿using Domain.Players;

namespace Domain.PlayingCards
{
    public class RemingtonCard : WeaponCard
    {
        protected override bool EqualsCore(PlayingCard other) => other is RemingtonCard;

        protected override int GetHashCodeCore() => typeof(Remington).GetHashCode();
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}