using Domain.PlayingCards.Visitors;

namespace Domain.PlayingCards
{
    public class WinchesterCard : WeaponCard
    {
        public override string Description => CardName.Winchester;
        protected override bool EqualsCore(PlayingCard other) => other is WinchesterCard;
        protected override int GetHashCodeCore() => typeof(WinchesterCard).GetHashCode();
        public override T Accept<T>(ILongTermCardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}