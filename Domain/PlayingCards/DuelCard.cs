namespace Domain.PlayingCards
{
    public class DuelCard : PlayAndDiscardCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is DuelCard;
        }

        protected override int GetHashCodeCore()
        {
            throw new System.NotImplementedException();
        }
    }
}