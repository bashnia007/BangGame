﻿using Bang.Characters;
using Bang.Players;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;
using Bang.Weapons;
using Bang.Game;
using FluentAssertions;
using Xunit;

namespace Bang.Tests
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
        public void When_player_doesnt_have_weapon_card_then_his_weapon_is_colt(CardType weaponCardType)
        {
            // Arrange
            var weaponCard = weaponCardType.DiamondsThree();
            var tablet = CreateTablet();
            tablet.PutCard(weaponCard);
            
            // Act
            tablet.RemoveCard(weaponCard);
            
            // Assert
            tablet.Weapon.Should().Be(new Colt());
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.LongTermCards), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_copy_of_card_in_play(CardType cardType)
        {
            // Arrange
            var tablet = CreateTablet();
            var card = cardType.ClubsSeven();
            var otherCardWithSameType = cardType.SpadesQueen();
            tablet.PutCard(card);
            
            // Act
            var result = tablet.CanPutCard(otherCardWithSameType);
            
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DifferentWeapons), MemberType = typeof(TestDataGenerator))]
        public void Player_can_have_only_one_weapon_in_play(CardType oldWeapon, CardType newWeapon)
        {
            // Arrange
            var oldWeaponCard = oldWeapon.DiamondsThree();
            var newWeaponCard = newWeapon.SpadesQueen();
         
            var tablet = CreateTablet();
            tablet.PutCard(oldWeaponCard);
            
            // Act 
            var result = tablet.CanPutCard(newWeaponCard);
            
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.PlayAndDiscardCards), MemberType = typeof(TestDataGenerator))]
        public void Player_can_not_put_one_time_card_on_the_board(CardType cardType)
        {
            // Arrange
            var card = cardType.SpadesQueen();
            var tablet = CreateTablet();

            tablet.CanPutCard(card).Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.DifferentWeapons), MemberType = typeof(TestDataGenerator))]
        public void Player_can_change_weapon(CardType oldWeapon, CardType newWeapon)
        {
            // Arrange
            var oldWeaponCard = oldWeapon.DiamondsThree();
            var newWeaponCard = newWeapon.SpadesQueen();

            var weapon = newWeapon.Accept(new CardToWeaponVisitor());

            var tablet = CreateTablet();
            tablet.PutCard(oldWeaponCard);

            // Act
            tablet.ChangeWeapon(newWeaponCard);

            Assert.Equal(weapon, tablet.Weapon);
        }
    }
}