using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class ReplenishHandCardMessage : TakeCardsMessage
    {
        public BangGameCard ReplenishHandCard { get; }

        public ReplenishHandCardMessage(BangGameCard replenishHand) : base()
        {
            if (replenishHand.Type is StagecoachCardType)
            {
                CardsToTakeAmount = 2;
            }
            if (replenishHand.Type is WellsFargoCardType)
            {
                CardsToTakeAmount = 3;
            }
            ReplenishHandCard = replenishHand;
        }

        public ReplenishHandCardMessage(List<BangGameCard> cards) : base(cards) { }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessReplenishHandMessage(this);
        }
    }
}
