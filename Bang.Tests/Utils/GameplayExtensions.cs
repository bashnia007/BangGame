using System;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.Roles;

namespace Bang.Tests
{
    public static class GameplayExtensions
    {
        internal static Player SetTurnToPlayer(this Gameplay gameplay, Player player)
        {
            return SetTurnToCharacter(gameplay, player.Character);
        }

        internal static Player SetTurnToCharacter(this Gameplay gameplay, Character character)
        {
            if (gameplay.AlivePlayers.All(p => p.Character != character))
                throw new ArgumentException($"There isn't {character} in game!");

            while (gameplay.PlayerTurn.Character != character)
            {
                gameplay.StartPlayerTurn();
                gameplay.PlayerTurn.FinishTurn();
            }
            
            return gameplay.PlayerTurn;
        }

        internal static Player FindPlayer(this Gameplay gameplay, Character character)
        {
            return gameplay.AlivePlayers.First(p => p.Character == character);
        }

        internal static Player FindPlayer(this Gameplay gameplay, Role role)
        {
            return gameplay.AlivePlayers.First(p => p.Role == role);
        }

        internal static Player FindPlayerAtDistanceFrom(this Gameplay gameplay, int distance, Player player)
        {
            var alivePlayers = gameplay.AlivePlayers.ToList();
            foreach (var p in alivePlayers)
            {
                if (player == p) continue;

                if (DistanceCalculator.GetDistance(alivePlayers, player, p) == distance)
                    return p;
            }
            
            throw new InvalidOperationException($"There is no one player at distance {distance} from {player.Name}");
        }
    }
}