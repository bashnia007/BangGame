using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class CatBalouCard : PlayAndDiscardCard
    {
        public override string Description => CardName.CatBalou; 
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is CatBalouCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(CatBalouCard).GetHashCode();
        }
    }
}