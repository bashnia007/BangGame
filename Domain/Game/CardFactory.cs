using System;
using Domain.PlayingCards;

namespace Domain.Game
{
    internal static class CardFactory
    {
        private static readonly Random Random = new Random();

        public static BangGameCard Create(Type card)
        {
            var suite = (Suite) Random.Next(0, 4);
            var rank = (Rank) Random.Next(0, 13);
            
            return new BangGameCard((CardType)Activator.CreateInstance(card), suite, rank);
        }
    }
}