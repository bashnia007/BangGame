using Domain.Characters.Visitors;
using Domain.Players;
using Domain.PlayingCards;

using System;
using System.Collections.Generic;
using System.Linq;

using static Domain.MathExtension;
using static System.Math;


namespace Domain.Game
{
    public static class DistanceCalculator
    {
        /// <summary>
        /// The  distance  between  two  players  is
        /// the minimum  number  of  places  between them,  counting  clockwise  or  counterclockwise
        ///
        /// When you have a mustang horse in play the distance between other players and you is increased by one.
        /// However, you still see other players at the normal distance
        ///
        /// When you have a scope card in play the distance all other players at a distance decreased by one.
        /// However, other players still see you at the normal distance.
        ///
        /// Distances less then one are considered to be one.
        ///
        /// Paul Regret is considered to have a mustang in play at all times. 
        /// Rose Doolan is considered to have a scope in play at all times. 
        /// </summary>
        /// <param name="alivePlayers">alive players</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static int GetDistance(IList<Player> alivePlayers, Player from, Player to)
        {
            if (alivePlayers == null)
                throw new ArgumentNullException(nameof(alivePlayers));
            
            if (alivePlayers.Any(p => !p.PlayerTablet.IsAlive) || alivePlayers.Count < 2)
                throw new ArgumentException(nameof(alivePlayers));
            
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            int fromIndex = alivePlayers.IndexOf(from);
            if (fromIndex == -1)
                throw new ArgumentException($"Player {from.Name} is not alive");

            int toIndex = alivePlayers.IndexOf(to);
            if (toIndex == -1)
                throw new ArgumentException($"Player {to.Name} is not alive");

            int distance = Min(
                        Mod(toIndex - fromIndex, alivePlayers.Count),
                        Mod(fromIndex - toIndex, alivePlayers.Count)
                );

            if (from.PlayerTablet.ActiveCards.Any(c => c == new ScopeCardType()))
                distance--;

            if (to.PlayerTablet.ActiveCards.Any(c => c == new MustangCardType()))
                distance++;

            distance +=
                from.PlayerTablet.Character.Accept(new DistanceFromPlayerVisitor()) +
                to.PlayerTablet.Character.Accept(new DistanceToPlayerVisitor());

            return Max(distance, 1);
        }
    }
}