using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests.Characters
{
    public class ButchCassidySpecification
    {
        [Fact]
        public void Butch_Cassidy_starts_game_with_4_life_points()
        {
            var (gamePlay, butch) = CreateGamePlayWithButch();
            butch.AsOutlaw(gamePlay);
            
            // Assert
            butch.LifePoints.Should().Be(4);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void When_Butch_Cassidy_loses_a_life_point_he_draws_a_card(int lifePoints)
        {
            var (_, butch) = CreateGamePlayWithButch();

            var hand = butch.Hand.Count;
            
            // Act
            butch.LoseLifePoint(lifePoints);

            butch.Hand.Count.Should().Be(hand + lifePoints);
        }
        
        [Fact]
        public void When_Butch_Cassidy_loses_a_life_point_he_draws_a_card_from_the_deck()
        {
            var (gamePlay, butch) = CreateGamePlayWithButch();

            var cardFromDeck = gamePlay.PeekTopCardFromDeck();
            
            // Act
            butch.LoseLifePoint();

            butch.Hand.Should().Contain(cardFromDeck);
        }
        
        [Fact]
        public void When_Butch_Cassidy_loses_last_life_point_he_does_not_draw_card()
        {
            var (gamePlay, butch) = CreateGamePlayWithButch();

            butch.LoseLifePoint(butch.MaximumLifePoints - 1);
            
            var topDeckCard = gamePlay.PeekTopCardFromDeck();
            
            // Act
            butch.LoseLifePoint();

            gamePlay.PeekTopCardFromDeck().Should().Be(topDeckCard);
        }
        
        [Fact]
        public void Butch_Cassidy_draws_card_only_if_he_is_still_alive()
        {
            var (gamePlay, butch) = CreateGamePlayWithButch();

            var topDeckCard = gamePlay.PeekTopCardFromDeck();
            
            // Act
            butch.Die();

            gamePlay.PeekTopCardFromDeck().Should().Be(topDeckCard);
        }

        private (Gameplay, Player) CreateGamePlayWithButch()
        {
            var character = new BartCassidy();
            var gamePlay = InitGameplayWithCharacter(character);
            var butch = gamePlay.Players.First(p => p.Character == character);

            return (gamePlay, butch);
        }
    }
}