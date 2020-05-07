using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class CatBalouCardType : CardType
    {
        public override string Description => CardName.CatBalou;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is CatBalouCardType;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(CatBalouCardType).GetHashCode();
        }
    }
}