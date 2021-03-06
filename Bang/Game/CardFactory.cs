using System;
using Bang.PlayingCards;

namespace Bang.Game
{
    internal static class CardFactory
    {
        private static readonly Random Random = new Random();

        public static BangGameCard Create(CardType cardType)
        {
            var suite = (Suite) Random.Next(0, 4);
            var rank = (Rank) Random.Next(0, 13);
            
            return new BangGameCard(cardType, suite, rank);
        }
    }
}