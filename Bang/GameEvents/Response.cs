using System;
using System.Collections.Generic;
using Bang.GameEvents.Enums;
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

    public class DefenceAgainstDuel : Response
    {
        public override bool IsDone => false;
        public BangGameCard Card { get; set; }
    }

    public class DefenceAgainstIndians : Response
    {
        public override bool IsDone => false;
        public BangGameCard Card { get; set; }
    }

    public class ForcePlayerToDropCardResponse : Response
    {
        public override bool IsDone => false;
        public BangGameCard ActiveCardToDrop { get; private set; }
        public bool RandomHandCard => ActiveCardToDrop == null;

        public ForcePlayerToDropCardResponse(BangGameCard card)
        {
            ActiveCardToDrop = card;
        }
        
        public ForcePlayerToDropCardResponse() : this(null){}
    }

    public class DrawCardFromPlayerResponse : Response
    {
        public override bool IsDone => false;
        
        public BangGameCard ActiveCardToSteal { get; }
        public bool RandomHandCard => ActiveCardToSteal == null;

        public DrawCardFromPlayerResponse(BangGameCard card)
        {
            ActiveCardToSteal = card;
        }
        
        public DrawCardFromPlayerResponse() : this(null){}
    }

    public class NotAllowedOperation : Response
    {
        public override bool IsDone => true;
        public string Reason { get; }
        
        public NotAllowedOperation(){}

        public NotAllowedOperation(string reason)
        {
            Reason = reason;
        }
    }

    public class LeaveCardOnTheTableResponse : Response
    {
        public override bool IsDone => true;
    }

    public class MultiplayerDefenceResponse : Response
    {
        public override bool IsDone => false;
        public CardType CardTypeRequired { get; set; }
        public List<Response> PlayersResponses { get; set; }
    }

    public class ChooseCardsResponse : Response
    {
        public override bool IsDone => false;

        public List<BangGameCard> CardsToChoose { get; set; }
        public Player PlayerTurn { get; set; }
    }

    public class ChooseDrawOptionResponse : Response
    {
        public override bool IsDone => false;
        public DrawOptions DrawOption { get; set; }
    }

    public class TakeCardAfterGeneralStoreResponse : Response
    {
        public override bool IsDone => false;

        public TakeCardAfterGeneralStoreResponse(Player player, BangGameCard card)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Card = card ?? throw new ArgumentNullException(nameof(card));
        }
        
        public BangGameCard Card { get; }
    }
    
    public class GameOverResponse : Response
    {
        public override bool IsDone => true;
        public List<Player> Winners;

        public GameOverResponse(List<Player> winners)
        {
            Winners = winners;
        }
    }
}