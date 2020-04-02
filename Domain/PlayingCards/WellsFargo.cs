namespace Domain.PlayingCards
{
    public class WellsFargo : PlayAndDiscardCard
    {
        public override string Description => CardName.WellsFargo;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is WellsFargo;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WellsFargo).GetHashCode();
        }
    }
}