namespace Domain.PlayingCards
{
    public class WellsFargoCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is WellsFargoCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WellsFargoCard).GetHashCode();
        }
    }
}
