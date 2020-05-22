using System.Collections.Generic;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    public class ChooseOneCardResponse : Response
    {
        public List<BangGameCard> ActiveCards { get; set; }
        public List<ClosedHandCard> HandCardCodes { get; set; }
        public override bool IsDone => false;
    }
}