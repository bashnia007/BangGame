using System;
using Domain.PlayingCards;

namespace Domain.Game
{
    internal static class CardFactory
    {
        private static Random Random = new Random();

        public static BangGameCard Create(Type card)
        {
            var suite = (Suite) Random.Next(0, 4);
            var rank = (Rank) Random.Next(0, 4);
            
            return new BangGameCard((CardType)Activator.CreateInstance(card), suite, rank);
        }
    }
}