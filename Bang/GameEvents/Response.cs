using System.Collections.Generic;
using System.Data.SqlTypes;
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
}