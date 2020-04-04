namespace Domain.PlayingCards
{
    public class GeneralStoreCard : PlayAndDiscardCard
    {
        public override string Description => CardName.GeneralStore;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is GeneralStoreCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GeneralStoreCard).GetHashCode();
        }
    }
}