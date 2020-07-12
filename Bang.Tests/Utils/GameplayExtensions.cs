using System;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;

namespace Bang.Tests
{
    public static class GameplayExtensions
    {
        internal static Player SetTurnToPlayer(this Gameplay gameplay, Player player)
        {
            return SetTurnToCharacter(gameplay, player.Character);
        }

        internal static Gameplay SkipTurnsUntilPlayer(this Gameplay gameplay, Player player)
        {
            if (gameplay.PlayerTurn == player)
                gameplay.SetNextPlayer();

            while (gameplay.GetNextPlayer() != player)
            {
                gameplay.SetNextPlayer();
            }

            return gameplay;
        }
        
        internal static Player SetTurnToCharacter(this Gameplay gameplay, Character character)
        {
            if (gameplay.AlivePlayers.All(p => p.Character != character))
                throw new ArgumentException($"There isn't {character} in game!");
            
            while (gameplay.GetNextPlayer().Character != character)
            {
                gameplay.SetNextPlayer();
            }
            
            gameplay.SetNextPlayer();

            return gameplay.PlayerTurn;
        }

        internal static Player FindPlayerWithCharacter(this Gameplay gameplay, Character character)
        {
            return gameplay.AlivePlayers.First(p => p.Character == character);
        }
    }
}