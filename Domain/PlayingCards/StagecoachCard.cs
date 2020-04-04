namespace Domain.PlayingCards
{
    public class StagecoachCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is StagecoachCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(StagecoachCard).GetHashCode();
        }
    }
}