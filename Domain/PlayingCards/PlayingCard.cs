namespace Domain.PlayingCards
{
    public abstract class PlayingCard : ValueObject<PlayingCard>
    {
        public Suite Suite { get; set; } 
        public Rank Rank { get; set; } 
        public abstract bool PlayAndDiscard { get; }
    }
}