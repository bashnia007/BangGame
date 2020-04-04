namespace Domain.PlayingCards
{
    public class PanicCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Panic;
        
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