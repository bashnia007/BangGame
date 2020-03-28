namespace Domain.PlayingCards
{
    public class SaloonCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SaloonCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SaloonCard).GetHashCode();
        }
    }
}