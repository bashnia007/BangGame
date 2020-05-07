using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    [Serializable]
    public class StagecoachCardType : CardType
    {
        public override string Description => CardName.Stagecoach;
        public override T Accept<T>(ICardTypeVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(CardType other)
        {
            return other is StagecoachCardType;
        }

        protected override int GetHashCodeCore()
        {
            return (typeof(StagecoachCardType)).GetHashCode();
        }
    }
}