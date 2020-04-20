using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class WellsFargoMessage : ReplenishHandCardMessage
    {
        public WellsFargoMessage(BangGameCard card) : base(card)
        {
            CardsToTakeAmount = 3;
        }

        public WellsFargoMessage(List<BangGameCard> cards) : base(cards) { }
    }
}
