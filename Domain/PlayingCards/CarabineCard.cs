using Domain.Players;

namespace Domain.PlayingCards
{
    public class CarabineCard : WeaponCard
    {
        protected override bool EqualsCore(PlayingCard other) => other is CarabineCard;

        protected override int GetHashCodeCore() => typeof(CarabineCard).GetHashCode();
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}