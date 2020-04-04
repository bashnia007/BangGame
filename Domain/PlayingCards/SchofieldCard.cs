using Domain.Players;

namespace Domain.PlayingCards
{
    public class SchofieldCard : WeaponCard
    {
        public override string Description => CardName.Schofield;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SchofieldCard;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SchofieldCard).GetHashCode();
        }

        public override T Accept<T>(ICardVisitor<T> visitor) => visitor.Visit(this);
        
    }
}