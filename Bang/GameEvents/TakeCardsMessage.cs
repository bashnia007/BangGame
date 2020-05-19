using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents
{
    public class TakeCardsEvent : BangGameMessage
    {
        public List<BangGameCard> PlayingCards { get; }
        public short CardsToTakeAmount { get; protected set; }

        public TakeCardsEvent(short cardsToTakeAmount, Player player) : base(player)
        {
            CardsToTakeAmount = cardsToTakeAmount;
        }

        public override Response Handle(BangEventsHandler handler)
        {
            return handler.DealCard(this);
        }
    }
}
