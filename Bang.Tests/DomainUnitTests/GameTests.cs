using Domain.Game;
using Domain.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class GameTests
    {
        [Fact]
        public void The_sheriff_plays_the_game_with_one_additional_life()
        {
            // Arrange
            var players = CreatePlayers(4);
            var game = new Game(players[0]);

            for (int i = 1; i < 4; i++)
            {
                game.JoinPlayer(players[i]);
            }

            game.Initialize();
            
            // Act
            var sheriffPlayer = game.Players.First(p => p.PlayerTablet.IsSheriff);
            
            // Assert
            Assert.Equal(sheriffPlayer.PlayerTablet.Character.LifePoints + 1, sheriffPlayer.PlayerTablet.Health);
        }

        private List<Player> CreatePlayers(int amount)
        {
            var result = new List<Player>();

            for (int i = 0; i < amount; i++)
            {
                var id = new Guid().ToString();
                result.Add(new PlayerOnline(id));
            }

            return result;
        }
    }
}
