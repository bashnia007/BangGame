using Bang.Characters;
using FluentAssertions;
using Xunit;

namespace Bang.Tests
{
    public class TestUtilsTests
    {
        
        [Fact]
        public void SetTurnToCharacter()
        {
            var calamityJanet = new CalamityJanet();
            var gameplay = TestUtils.InitGameplayWithCharacter(calamityJanet);
            var player = gameplay.SetTurnToCharacter(calamityJanet);

            player.Character.Should().Be(calamityJanet);
        }
        
        // [Fact]
        // public void SkipTurnsUntilPlayer()
        // {
        //     var calamityJanet = new CalamityJanet();
        //     var gameplay = TestUtils.InitGameplayWithCharacter(calamityJanet);
        //     var player = gameplay.SkipTurnsUntilPlayer(calamityJanet);
        //
        //     player.Character.Should().Be(calamityJanet);
        // }
    }
}