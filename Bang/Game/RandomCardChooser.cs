using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bang.PlayingCards;

namespace Bang.Game
{
    internal static class RandomCardChooser
    {
        private static readonly Random random = new Random();
        
        internal static BangGameCard ChooseCard(IReadOnlyList<BangGameCard> victimHand)
        {
            Debug.Assert(victimHand != null, "Victim hand can not be null");

            if (victimHand.Count == 0)
                return null;
            
            int number = random.Next(victimHand.Count);
            return victimHand[number];
        }
    }
}