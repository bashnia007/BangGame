using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class BangCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Bang; 
        protected override bool EqualsCore(CardType other)
        {
            return other is BangCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BangCardType).GetHashCode();
        }
    }
}