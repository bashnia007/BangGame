using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class CatBalouCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.CatBalou; 
        
        protected override bool EqualsCore(CardType other)
        {
            return other is CatBalouCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(CatBalouCardType).GetHashCode();
        }
    }
}