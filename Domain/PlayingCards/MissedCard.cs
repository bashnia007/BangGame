namespace Domain.PlayingCards
{
    public class MissedCard : PlayAndDiscardCard
    {
        public override string Description => CardName.Missed;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is MissedCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MissedCard).GetHashCode();
        }
    }
}