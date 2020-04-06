using Domain.Character;
using Domain.Players;
using Domain.PlayingCards;
using Domain.Weapons;
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
        public void Player_starts_with_colt_weapon()
        {
            var tablet = CreateTablet();
            
            Assert.Equal(new Colt(), tablet.Weapon);
        }
        
        [Theory]
        [MemberData(nameof(TestDataGenerator.WeaponCards), MemberType = typeof(TestDataGenerator))]
        public void When_player_doesnt_have_weapon_card_then_his_weapon_is_colt(WeaponCard weaponCard)
        {
            // Arrange 
            var tablet = CreateTablet();
            tablet.PutCard(weaponCard);
            
            // Act
            tablet.RemoveCard(weaponCard);
            
            // Assert
            Assert.Equal(new Colt(), tablet.Weapon);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LongTermCards), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_copy_of_card_in_play(LongTermFeatureCard card)
        {
            // Arrange
            var tablet = CreateTablet();
            tablet.PutCard(card);
            
            // Act
            var result = tablet.CanPutCard(card);
            
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DifferentWeapons), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_weapon_in_play(WeaponCard oldWeapon, WeaponCard newWeapon)
        {
            // Arrange
            var tablet = CreateTablet();
            tablet.PutCard(oldWeapon);
            
            // Act 
            var result = tablet.CanPutCard(newWeapon);
            
            Assert.False(result);
        }
    }
}