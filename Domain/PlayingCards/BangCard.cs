namespace Domain.PlayingCards
{
    public class BangCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is BangCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BangCard).GetHashCode();
        }
    }
}