namespace Domain.PlayingCards
{
    public class DynamiteCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is DynamiteCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DynamiteCard).GetHashCode();
        }
    }
}