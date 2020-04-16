using System;
using Domain.PlayingCards;

namespace Domain.Game
{
    public static class CardFactory
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