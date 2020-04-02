namespace Domain.PlayingCards
{
    public class BangCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Bang; 
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