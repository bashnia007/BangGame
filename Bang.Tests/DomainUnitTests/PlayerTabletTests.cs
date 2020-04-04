using Domain.Character;
using Domain.Players;
using Domain.PlayingCards;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class PlayerTabletTests
    {
        private PlayerTablet CreateTablet()
        {
            return new PlayerTablet(new Jourdonnais(), false);
        }
        
        [Fact]
        public void When_player_plays_weapon_card_then_previous_weapon_dropped()
        {
            // Arrange
            var tablet = CreateTablet();
            tablet.PutCard(new RemingtonCard());
            
            // Act
            tablet.PutCard(new SchofieldCard());
            
            Assert.DoesNotContain(new RemingtonCard(), tablet.LongTermFeatureCards);
        }

        [Fact]
        public void When_player_doesnt_have_weapon_card_then_his_weapon_is_colt()
        {
            // Arrange 
            var tablet = CreateTablet();
            
            Assert.Equal(new Colt(), tablet.Weapon);
        }
    }
}