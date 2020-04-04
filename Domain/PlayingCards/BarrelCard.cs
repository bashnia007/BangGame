using Domain.Players;

namespace Domain.PlayingCards
{
    public class BarrelCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is BarrelCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BarrelCard).GetHashCode();
        }

        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}