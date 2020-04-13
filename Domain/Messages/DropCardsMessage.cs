using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class DropCardsMessage : Message
    {
        public List<PlayingCard> CardsToDrop { get; }

        public DropCardsMessage(List<PlayingCard> cardsToDrop)
        {
            CardsToDrop = cardsToDrop;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessDropCardsMessage(this);
        }
    }
}
