using System;

namespace Domain.PlayingCards
{
    [Serializable]
    public abstract class CardType : ValueObject<CardType>
    {
        public abstract string Description { get; } 
        public abstract bool PlayAndDiscard { get; }
    }
}