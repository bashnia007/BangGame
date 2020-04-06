using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public class StagecoachCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Stagecoach;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is StagecoachCard;
        }

        protected override int GetHashCodeCore()
        {
            return (typeof(StagecoachCard)).GetHashCode();
        }
    }
}