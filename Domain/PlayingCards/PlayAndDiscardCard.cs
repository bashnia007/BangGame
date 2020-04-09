using System;

namespace Domain.PlayingCards
{
    [Serializable]
    /// <summary>
    /// Cards played by putting them directly into the discard pile and
    /// applying the effect described with text or with symbols on the cards
    /// </summary>
    public abstract class PlayAndDiscardCard : PlayingCard
    {
        public override bool PlayAndDiscard => true;
    }
}