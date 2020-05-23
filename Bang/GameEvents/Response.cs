using System.Collections.Generic;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents
{
    public abstract class Response
    {
        public abstract bool IsDone { get; }
        public BangGameMessage ReplyTo { get; set; }
        public Player Player { get; set; }
    }

    public class Done : Response
    {
        public override bool IsDone => true;
    }
    
    public class DefenceAgainstBang : Response
    {
        public override bool IsDone => false;
        public BangGameCard FirstCard { get; set; }
        public BangGameCard SecondCard { get; set; }
        public byte CardsRequired { get; set; }
    }

    public class NotAllowedOperation : Response
    {
        public override bool IsDone => true;
    }

    public class MultiplayerDefenceResponse : Response
    {
        public override bool IsDone => false;
        public BangGameCard CardTypeRequired { get; set; }
        public List<DefenceAgainstBang> PlayersResponses { get; set; }
    }
}