using Domain.Game;
using Domain.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class GameInitializationTests
    {
        [Fact]
        public void InitializePlayingCards_Always_CreatesEightyCards()
        {
            Assert.Equal(80, GameInitializer.PlayingCards.Count);
        }

        [Fact]
        public void InitializeCharacters_Always_CreatesSixteenCharacters()
        {
            Assert.Equal(16, GameInitializer.Characters.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(8)]
        public void CreateRolesForGame_WithIncorrectPlayersAmount_ThrowsException(int playersAmount)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var gameSet = GameInitializer.CreateRolesForGame(playersAmount);
            });
        }

        [Theory]
        [InlineData(4, 2, 0)]
        [InlineData(5, 2, 1)]
        [InlineData(6, 3, 1)]
        [InlineData(7, 3, 2)]
        public void CreateRolesForGame_Creates_ProperlyRoles(int playersAmount, int outlawExpected, int deputyExpected)
        {
            var roles = GameInitializer.CreateRolesForGame(playersAmount);
            Assert.Equal(outlawExpected, roles.Where(x => x.GetType() == typeof(Outlaw)).Count());
            Assert.Equal(deputyExpected, roles.Where(x => x.GetType() == typeof(Deputy)).Count());
            Assert.Single(roles.Where(x => x.GetType() == typeof(Renegade)));
            Assert.Single(roles.Where(x => x.GetType() == typeof(Sheriff)));
        }
    }
}
