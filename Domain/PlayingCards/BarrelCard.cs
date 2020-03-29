namespace Domain.PlayingCards
{
    public class BarrelCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is BarrelCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BarrelCard).GetHashCode();
        }
    }
}