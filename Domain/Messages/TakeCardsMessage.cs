using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class TakeCardsMessage : Message
    {
        public List<BangGameCard> PlayingCards { get; }
        public short CardsToTakeAmount { get; }

        public TakeCardsMessage(List<BangGameCard> playingCards)
        {
            PlayingCards = playingCards;
        }

        public TakeCardsMessage(short cardsToTakeAmount)
        {
            CardsToTakeAmount = cardsToTakeAmount;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessTakeCardsMessage(this);
        }
    }
}
