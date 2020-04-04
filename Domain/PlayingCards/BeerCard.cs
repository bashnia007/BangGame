namespace Domain.PlayingCards
{
    public class BeerCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Beer;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is BeerCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BeerCard).GetHashCode();
        }
    }
}