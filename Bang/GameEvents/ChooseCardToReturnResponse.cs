using Bang.PlayingCards;

namespace Bang.GameEvents
{
    public class ChooseCardToReturnResponse : Response
    {
        public BangGameCard CardToReturn { get; set; }
        public override bool IsDone => false;
    }
}
