using System;
using Domain.Characters;
using Domain.Game;
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
        public void When_player_doesnt_have_weapon_card_then_his_weapon_is_colt(WeaponCardType weaponCardType)
        {
            // Arrange 
            var tablet = CreateTablet();
            tablet.PutCard(CardFactory.Create(weaponCardType));
            
            // Act
            tablet.RemoveCard(CardFactory.Create(weaponCardType));
            
            // Assert
            Assert.Equal(new Colt(), tablet.Weapon);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LongTermCards), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_copy_of_card_in_play(LongTermFeatureCardType cardType)
        {
            // Arrange
            var tablet = CreateTablet();
            tablet.PutCard(CardFactory.Create(cardType));
            
            // Act
            var result = tablet.CanPutCard(CardFactory.Create(cardType));
            
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DifferentWeapons), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_weapon_in_play(WeaponCardType oldWeapon, WeaponCardType newWeapon)
        {
            // Arrange
            var tablet = CreateTablet();
            tablet.PutCard(CardFactory.Create(oldWeapon));
            
            // Act 
            var result = tablet.CanPutCard(CardFactory.Create(newWeapon));
            
            Assert.False(result);
        }
    }
}