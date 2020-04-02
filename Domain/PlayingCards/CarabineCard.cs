﻿namespace Domain.PlayingCards
{
    public class CarabineCard : WeaponCard
    {
        public override int Distance => 4;
        public override bool MultipleBang => false;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is CarabineCard;
        }
    }
}