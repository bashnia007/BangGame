using Domain.Players;

namespace Domain.PlayingCards
{
    public class VolcanicCard : WeaponCard
    {
        protected override bool EqualsCore(PlayingCard other) => other is VolcanicCard;

        protected override int GetHashCodeCore() => typeof(Volcanic).GetHashCode();
        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
    }
}