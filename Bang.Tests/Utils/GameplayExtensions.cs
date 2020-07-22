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

        internal static Gameplay SkipTurnsUntilCharacter(this Gameplay gameplay, Character character)
        {
            if (gameplay.PlayerTurn.Character == character)
                gameplay.SetNextPlayer();

            while (gameplay.GetNextPlayer().Character != character)
            {
                gameplay.SetNextPlayer();
            }

            return gameplay;
        }
        internal static Gameplay SkipTurnsUntilPlayer(this Gameplay gameplay, Player player)
        {
            return SkipTurnsUntilCharacter(gameplay, player.Character);
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

        internal static Player FindPlayer(this Gameplay gameplay, Character character)
        {
            return gameplay.AlivePlayers.First(p => p.Character == character);
        }
    }
}