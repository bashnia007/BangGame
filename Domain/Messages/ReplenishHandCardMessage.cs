using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public abstract class ReplenishHandCardMessage : TakeCardsMessage
    {
        public BangGameCard ReplenishHandCard { get; }

        protected ReplenishHandCardMessage(BangGameCard replenishHand) : base()
        {
            ReplenishHandCard = replenishHand;
        }

        protected ReplenishHandCardMessage(List<BangGameCard> cards) : base(cards) { }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessReplenishHandMessage(this);
        }
    }
}
