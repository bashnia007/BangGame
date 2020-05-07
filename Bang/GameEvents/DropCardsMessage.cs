using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents
{
    public class DropCardsMessage : BangGameMessage
    {
        public List<BangGameCard> CardsToDrop { get; }
        public DropCardsMessage(List<BangGameCard> cardsToDrop, Player player) : base(player)
        {
            CardsToDrop = cardsToDrop;
        }

        public override Response Handle(BangEventsHandler handler)
        {
            return handler.DropCard(this);
        }
    }
}
