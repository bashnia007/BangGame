namespace Domain.PlayingCards
{
    public class GatlingCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is GatlingCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GatlingCard).GetHashCode();
        }
    }
}