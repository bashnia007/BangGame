using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class DropCardsMessage : Message
    {
        public List<BangGameCard> CardsToDrop { get; }

        public DropCardsMessage(List<BangGameCard> cardsToDrop)
        {
            CardsToDrop = cardsToDrop;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessDropCardsMessage(this);
        }
    }
}
