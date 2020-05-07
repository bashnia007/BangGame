using System;
using Bang.PlayingCards.Visitors;
using Bang;

namespace Bang.PlayingCards
{
    [Serializable]
    public abstract class CardType : ValueObject<CardType>
    {
        public abstract string Description { get; }
        public abstract T Accept<T>(ICardTypeVisitor<T> visitor);
    }
}