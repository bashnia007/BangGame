using Bang.PlayingCards;

namespace Bang.Tests
{
    public static class TestUtils
    {
        public static BangGameCard DiamondsThree(this CardType cardType) => new BangGameCard(cardType, Suite.Diamonds, Rank.Three);
        public static BangGameCard ClubsSeven(this CardType cardType) => new BangGameCard(cardType, Suite.Clubs, Rank.Seven);
        public static BangGameCard SpadesQueen(this CardType cardType) => new BangGameCard(cardType, Suite.Spades, Rank.Queen);
        public static BangGameCard HeartsAce(this CardType cardType) => new BangGameCard(cardType, Suite.Hearts, Rank.Ace);
    }
}