using Xunit;
using System.Linq;
using Bang.Exceptions;
using Bang.Game;
using Bang.Roles;

namespace Bang.Tests
{
    public class GameInitializationTests
    {
        [Fact]
        public void There_are_eighty_cards_in_the_game()
        {
            Assert.Equal(80, GameInitializer.PlayingCards.Count);
        }

        [Fact]
        public void There_are_sixteen_characters()
        {
            Assert.Equal(16, GameInitializer.Characters.Count);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(8)]
        public void Four_to_seven_players_can_play_the_game(int playersAmount)
        {
            Assert.Throws<AmountPlayersException>(() =>
            {
                var gameSet = GameInitializer.CreateRolesForGame(playersAmount);
            });
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void There_is_always_a_sheriff_in_the_game(int playersAmount)
        {
            var roles = GameInitializer.CreateRolesForGame(playersAmount);

            Assert.Single(roles.OfType<Sheriff>());
        }
        
        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void There_is_always_a_renegade_in_the_game(int playersAmount)
        {
            var roles = GameInitializer.CreateRolesForGame(playersAmount);

            Assert.Single(roles.OfType<Renegade>());
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(5, 1)]
        [InlineData(6, 1)]
        [InlineData(7, 2)]
        public void Amount_of_deputies_depends_on_the_players_amount(int playersAmount, int deputyExpected)
        {
            var roles = GameInitializer.CreateRolesForGame(playersAmount);

            Assert.Equal(deputyExpected, roles.OfType<Deputy>().Count());
        }
        
        [Theory]
        [InlineData(4, 2)]
        [InlineData(5, 2)]
        [InlineData(6, 3)]
        [InlineData(7, 3)]
        public void Amount_of_outlaws_depends_on_the_players_amount(int playersAmount, int outlawExpected)
        {
            var roles = GameInitializer.CreateRolesForGame(playersAmount);
            Assert.Equal(outlawExpected, roles.OfType<Outlaw>().Count());
        }
    }
}
