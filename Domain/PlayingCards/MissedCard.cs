namespace Domain.PlayingCards
{
    public class MissedCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is MissedCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MissedCard).GetHashCode();
        }
    }
}