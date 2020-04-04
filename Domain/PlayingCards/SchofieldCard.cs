﻿namespace Domain.PlayingCards
{
    public class SchofieldCard : WeaponCard
    {
        public override string Description => CardName.Schofield;
        
        public override int Distance => 2;
        public override bool MultipleBang => false;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SchofieldCard;
        }
    }
}