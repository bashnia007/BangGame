using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class StagecoachCardMessage : ReplenishHandCardMessage
    {
        public StagecoachCardMessage(BangGameCard replenishHand) : base(replenishHand)
        {
            CardsToTakeAmount = 2;
        }

        public StagecoachCardMessage(List<BangGameCard> cards) : base(cards) {  }
    }
}
