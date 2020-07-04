using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;

namespace Server.Tests
{
    public static class Utils
    {
        public static BangGameCard DiamondsThree(this CardType cardType) => new BangGameCard(cardType, Suite.Diamonds, Rank.Three);
        public static BangGameCard ClubsSeven(this CardType cardType) => new BangGameCard(cardType, Suite.Clubs, Rank.Seven);
        public static BangGameCard SpadesQueen(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Queen);
        public static BangGameCard HeartsAce(this CardType cardType) => new BangGameCard(cardType, Suite.Hearts, Rank.Ace);

        public static BangGameMessage ReplyWithDefenceAgainstIndians(this BangGameMessage replyTo, Player defender,
            BangGameCard card)
        {
            return new ReplyActionMessage(defender)
            {
                Response = new DefenceAgainstIndians
                {
                    Player = defender, 
                    Card = card, 
                    ReplyTo = replyTo
                }
            };
        }
        
        public static BangGameMessage ReplyWithDefenceAgainstIndians(this BangGameMessage replyTo, Player defender)
        {
            return ReplyWithDefenceAgainstIndians(replyTo, defender, null);
        }

        public static BangGameMessage ReplyWithTakingCardFromBoard(this BangGameMessage replyTo, Player player, BangGameCard card)
        {
            return new ReplyActionMessage(player)
            {
                Response = new TakeCardAfterGeneralStoreResponse(player, card){ReplyTo = replyTo}
            };
        }
        
        public static BangGameMessage ReplyWithDefenceAgainstBang(this BangGameMessage replyTo, Player player, BangGameCard card, BangGameCard secondCard = null)
        {
            return new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstBang(){Player = player, ReplyTo = replyTo, FirstCard = card, SecondCard = secondCard}
            };
        }
        
        public static BangGameMessage ReplyWithDefenceAgainstDuel(this BangGameMessage replyTo, Player player, BangGameCard card)
        {
            return new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstDuel(){Player = player, ReplyTo = replyTo, Card = card}
            };
        }

        public static BangGameMessage ReplyWithForcingToDrop(this BangGameMessage replyTo, Player player,
            BangGameCard card = null)
        {
            return new ReplyActionMessage(player)
            {
                Response = card == null?
                    new ForcePlayerToDropCardResponse() {Player = player, ReplyTo = replyTo} :
                    new ForcePlayerToDropCardResponse(card) {Player = player, ReplyTo = replyTo}
            };
        }
    }
}