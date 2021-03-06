using Bang.Characters;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class GameplayExtensionTests
    {
        [Fact]
        public void SetTurnToCharacter()
        {
            var calamityJanet = new CalamityJanet();
            var gameplay = InitGameplayWithCharacter(calamityJanet);
            var player = gameplay.SetTurnToCharacter(calamityJanet);

            player.Character.Should().Be(calamityJanet);
        }
    }
}