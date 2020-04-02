namespace Domain.PlayingCards
{
    public class BarrelCard : LongTermFeatureCard
    {
        public override string Description => CardName.Barrel;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is BarrelCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BarrelCard).GetHashCode();
        }
    }
}