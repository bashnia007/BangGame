﻿namespace Domain.PlayingCards
{
    public class RemingtonCard : WeaponCard
    {
        public override int Distance => 3;
        public override bool MultipleBang => false;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is RemingtonCard;
        }
    }
}