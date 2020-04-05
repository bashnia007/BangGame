using Domain.PlayingCards.Visitors;

namespace Domain.PlayingCards
{
    public class ScopeCard : LongTermFeatureCard
    {
        public override string Description => CardName.Scope;
        
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is ScopeCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(ScopeCard).GetHashCode();
        }
        
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
    }
}