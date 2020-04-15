using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class GeneralStoreCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.GeneralStore;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is GeneralStoreCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GeneralStoreCardType).GetHashCode();
        }
    }
}