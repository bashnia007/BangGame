using Domain.Game;
using Domain.Players;
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
            var game = new Game(CreatePlayers(4));
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
                result.Add(new PlayerOnline());
            }

            return result;
        }
    }
}
