using Domain.Game;
using Domain.Players;
using Domain.Role;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class GameTests
    {
        [Fact]
        public void GameInitialize_ForSheriff_ProvidesAdditionalLife()
        {
            var game = new Game(CreatePlayers(4));
            game.Initialize();
            var sheriffPlayer = game.Players.First(p => p.PlayerTablet.IsSheriff);
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
