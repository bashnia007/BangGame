using Domain.Characters;
using Domain.Players;
using FluentAssertions;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class LifePointsSpecification
    {
        private PlayerTablet CreateTablet(Character character, bool isSheriff = false)
        {
            return new PlayerTablet(character, isSheriff);
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void The_sheriff_plays_the_game_with_one_additional_life(Character character)
        {
            // Arrange and act
            var sheriffTablet = CreateTablet(character, true);
            
            // Assert
            sheriffTablet.Health.Should().Be(character.LifePoints + 1);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void Each_non_sheriff_player_life_points_are_equal_to_character_life_points(Character character)
        {
            var tablet = CreateTablet(character);

            tablet.Health.Should().Be(character.LifePoints);
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void Player_starts_the_game_with_maximum_life_points(Character character)
        {
            var tablet = CreateTablet(character);

            tablet.Health.Should().Be(tablet.MaximumHealth);
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void Player_life_points_have_limit(Character character)
        {
            // Arrange
            var tablet = CreateTablet(character);
            
            // Act
            tablet.Health = tablet.MaximumHealth + 1;
            
            // Assert
            tablet.Health.Should().Be(tablet.MaximumHealth);
        }
    }
}