using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class StagecoachCardType : PlayAndDiscardCardType
    {
        public override string Description => CardName.Stagecoach;
        
        protected override bool EqualsCore(CardType other)
        {
            return other is StagecoachCardType;
        }

        protected override int GetHashCodeCore()
        {
            return (typeof(StagecoachCardType)).GetHashCode();
        }
    }
}