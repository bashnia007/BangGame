using Domain.Players;

namespace Domain.PlayingCards
{
    public class MustangCard : LongTermFeatureCard
    {
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is MustangCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(MustangCard).GetHashCode();
        }
        
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}