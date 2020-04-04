using Domain.Players;

namespace Domain.PlayingCards
{
    public class DynamiteCard : LongTermFeatureCard
    {
        public override string Description => CardName.Dynamite;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is DynamiteCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DynamiteCard).GetHashCode();
        }

        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}