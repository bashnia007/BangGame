﻿namespace Domain.PlayingCards
{
    public class IndiansCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is IndiansCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(IndiansCard).GetHashCode();
        }
    }
}