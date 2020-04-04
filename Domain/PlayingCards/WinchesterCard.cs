using Domain.Players;

namespace Domain.PlayingCards
{
    public class WinchesterCard : WeaponCard
    {
        public override string Description => CardName.Winchester;
        protected override bool EqualsCore(PlayingCard other) => other is WinchesterCard;
        protected override int GetHashCodeCore() => typeof(Winchester).GetHashCode();
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}