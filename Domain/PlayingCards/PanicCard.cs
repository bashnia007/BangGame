namespace Domain.PlayingCards
{
    public class PanicCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is PanicCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(PanicCard).GetHashCode();
        }
    }
}