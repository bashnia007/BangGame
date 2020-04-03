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
        public void CreatePlayingCards_Always_CreatesEightyCards()
        {
            var gameSet = GameInitializer.CreateGameSet(4);
            Assert.Equal(80, gameSet.Deck.Count);
        }

        [Fact]
        public void CreateCharacters_Always_CreatesSixteenCharacters()
        {
            var gameSet = GameInitializer.CreateGameSet(4);
            Assert.Equal(16, gameSet.Characters.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(8)]
        public void CreateGame_WithIncorrectPlayersAmount_ThrowsException(int playersAmount)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var gameSet = GameInitializer.CreateGameSet(playersAmount);
            });
        }

        [Theory]
        [InlineData(4, 2, 0)]
        [InlineData(5, 2, 1)]
        [InlineData(6, 3, 1)]
        [InlineData(7, 3, 2)]
        public void CreateRoles_Creates_ProperlyRoles(int playersAmount, int outlawExpected, int deputyExpected)
        {
            var gameSet = GameInitializer.CreateGameSet(playersAmount);
            Assert.Equal(outlawExpected, gameSet.Roles.Where(x => x.GetType() == typeof(Outlaw)).Count());
            Assert.Equal(deputyExpected, gameSet.Roles.Where(x => x.GetType() == typeof(Deputy)).Count());
            Assert.Single(gameSet.Roles.Where(x => x.GetType() == typeof(Renegade)));
            Assert.Single(gameSet.Roles.Where(x => x.GetType() == typeof(Sheriff)));
        }
    }
}
