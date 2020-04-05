using Domain.PlayingCards.Visitors;

namespace Domain.PlayingCards
{
    public class JailCard : LongTermFeatureCard
    {
        public override string Description => CardName.Jail;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is JailCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JailCard).GetHashCode();
        }
        
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
    }
}