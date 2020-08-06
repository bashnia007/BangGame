using System;
using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents
{
    public class PlayCardMessage : BangGameMessage
    {
        public BangGameCard Card { get; }
        public BangGameCard AdditionalCard { get; }
        public Player PlayAgainst { get; }
        public override Response Handle(BangEventsHandler handler)
        {
            return handler.PlayCard(this);
        }

        public PlayCardMessage(Player actor, BangGameCard card, Player againstTo = null) : base(actor)
        {
            Card = card?? throw  new ArgumentNullException(nameof(card));
            PlayAgainst = againstTo;
        }

        public PlayCardMessage(Player actor, BangGameCard card, BangGameCard additionalCard)
            : this(actor, card, (Player) null)
        {
            AdditionalCard = additionalCard;
        }
    }
}