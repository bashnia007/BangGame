namespace Domain.PlayingCards
{
    public class GeneralStoreCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is GeneralStoreCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GeneralStoreCard).GetHashCode();
        }
    }
}