using Domain.Players;

namespace Domain.PlayingCards
{
    public class MustangCard : LongTermFeatureCard
    {
        public override string Description => CardName.Mustang;
        
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