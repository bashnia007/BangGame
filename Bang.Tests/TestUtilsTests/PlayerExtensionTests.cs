using System.Linq;
using Bang.Characters;
using FluentAssertions;
using Xunit;

namespace Bang.Tests.TestUtilsTests
{
    public class PlayerExtensionTests
    {
        [Fact]
        public void Player_WithOneLifePoint()
        {
            var player = TestUtils.InitGameplay().Players.First();

            // Act
            player.WithOneLifePoint();
            
            // Assert
            player.LifePoints.Should().Be(1);
        }
        
        [Fact]
        public void Player_Dies()
        {
            var player = TestUtils.InitGameplay().Players.First();

            // Act
            player.Die();
            
            // Assert
            player.LifePoints.Should().Be(0);
        }
    }
}