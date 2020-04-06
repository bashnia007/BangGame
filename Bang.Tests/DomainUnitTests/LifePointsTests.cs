using Domain.Character;
using Domain.Players;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class LifePointsTests
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
            Assert.Equal(character.LifePoints + 1, sheriffTablet.Health);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void Each_non_sheriff_player_life_points_are_equal_to_character_life_points(Character character)
        {
            var tablet = CreateTablet(character);
            
            Assert.Equal(character.LifePoints, tablet.Health);
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.AllCharacters), MemberType = typeof(TestDataGenerator))]
        public void Player_starts_the_game_with_maximum_life_points(Character character)
        {
            var tablet = CreateTablet(character);
            
            Assert.Equal(tablet.MaximumHealth, tablet.Health);
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
            Assert.Equal(tablet.MaximumHealth, tablet.Health);
        }
    }
}