﻿using Domain.Players;

namespace Domain.PlayingCards
{
    public class JailCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is JailCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JailCard).GetHashCode();
        }
        
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}