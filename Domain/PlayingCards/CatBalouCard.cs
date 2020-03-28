namespace Domain.PlayingCards
{
    public class CatBalouCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is CatBalouCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(CatBalouCard).GetHashCode();
        }
    }
}