using System;
using System.Collections.Generic;
using Bang.GameEvents.CardEffects;
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

    public class ForceCardToDrop : Response
    {
        public override bool IsDone => false;
        public BangGameCard ActiveCardToDrop { get; private set; }
        public ClosedHandCard HandCardToDrop { get; private set; }

        public ForceCardToDrop(BangGameCard card)
        {
            ActiveCardToDrop = card ?? throw new ArgumentNullException();
        }

        public ForceCardToDrop(ClosedHandCard handCard)
        {
            HandCardToDrop = handCard ?? throw new ArgumentNullException();
        }
    }

    public class NotAllowedOperation : Response
    {
        public override bool IsDone => true;
    }
}