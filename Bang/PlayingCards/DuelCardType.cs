using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class DuelCardType : CardType
    {
        public override string Description => CardName.Duel;
        

        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);
        protected override bool EqualsCore(CardType other)
        {
            return other is DuelCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(DuelCardType).GetHashCode();
        }
    }
}