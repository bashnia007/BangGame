using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class GeneralStoreCardType : CardType
    {
        public override string Description => CardName.GeneralStore;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is GeneralStoreCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(GeneralStoreCardType).GetHashCode();
        }
    }
}