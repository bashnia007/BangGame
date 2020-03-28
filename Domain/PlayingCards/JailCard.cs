namespace Domain.PlayingCards
{
    public class JailCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is JailCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JailCard).GetHashCode();
        }
    }
}