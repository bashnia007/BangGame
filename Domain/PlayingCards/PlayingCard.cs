namespace Domain.PlayingCards
{
    public abstract class PlayingCard : ValueObject<PlayingCard>
    {
        public abstract bool PlayAndDiscard { get; }
    }
}