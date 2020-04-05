using System.Collections.Generic;
using System.Linq;
using Domain.Character;
using Domain.Players;
using Domain.PlayingCards;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class PlayerTabletTests
    {
        public static IEnumerable<object[]> WeaponCards =>
            new List<object[]>
            {
                new []{new VolcanicCard()},
                new []{new SchofieldCard()},
                new []{new RemingtonCard()},
                new []{new CarabineCard()},
                new []{new WinchesterCard()}
            };

        public static IEnumerable<object[]> LongTermCardsExceptWeapon
        {
            get
            {
                return 
                    new List<object[]>
                    {
                        new[] {new ScopeCard()},
                        new[] {new MustangCard()},
                        new[] {new BarrelCard()},
                        new[] {new JailCard()},
                        new[] {new DynamiteCard()}
                    };
            }
        }

        public static IEnumerable<object[]> LongTermCards => LongTermCardsExceptWeapon.Concat(WeaponCards);
        
        public static IEnumerable<object[]> DifferentWeapons
        {
            get
            {
                var weapons = new List<WeaponCard>
                {
                    new VolcanicCard(),
                    new SchofieldCard(),
                    new RemingtonCard(),
                    new CarabineCard(),
                    new WinchesterCard()
                };
                
                foreach (var firstWeapon in weapons)
                {
                    foreach (var secondWeapon in weapons)
                    {
                        if (firstWeapon == secondWeapon) continue;
                        
                        yield return new object[]{firstWeapon, secondWeapon};
                    }
                }
                yield break;
            }
        }

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
        [MemberData(nameof(WeaponCards))]
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
        [MemberData(nameof(LongTermCards))]
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
        [MemberData(nameof(DifferentWeapons))]
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