using System.Collections.Generic;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;
using Bang.Weapons;
using Bang.Game;
using Xunit;

namespace Bang.Tests
{
    public class WeaponTests
    {
        public static IEnumerable<object[]> CardsToWeaponMapping
        {
            get
            {
                return new[]
                {
                    new object[] {new VolcanicCardType(), new Volcanic()},
                    new object[] {new SchofieldCardType(), new Schofield(),},
                    new object[] {new RemingtonCardType(), new Remington()},
                    new object[] {new CarabineCardType(), new Carabine(),},
                    new object[] {new WinchesterCardType(), new Winchester(),},
                };
            }
        }

        public static IEnumerable<object[]> WeaponToDistanceMapping
        {
            get
            {
                return new[]
                {
                    new object[] {new Colt(), 1},
                    new object[] {new Volcanic(), 1},
                    new object[] {new Schofield(), 2},
                    new object[] {new Remington(), 3},
                    new object[] {new Carabine(), 4},
                    new object[] {new Winchester(), 5}
                };
            }
        }
        
        public static IEnumerable<object[]> WeaponToMultipleBangMapping
        {
            get
            {
                return new[]
                {
                    new object[] {new Colt(), false},
                    new object[] {new Volcanic(), true},
                    new object[] {new Schofield(), false},
                    new object[] {new Remington(), false},
                    new object[] {new Carabine(), false},
                    new object[] {new Winchester(), false}
                };
            }
        }

        [Theory]
        [MemberData(nameof(CardsToWeaponMapping))]
        public void Cards_to_weapon_mapping(CardType weaponCardType, Weapon weapon)
        {
            Assert.Equal(weapon, weaponCardType.Accept(new CardToWeaponVisitor()));
        }

        [Theory]
        [MemberData(nameof(WeaponToDistanceMapping))]
        public void Weapon_to_distance_mapping(Weapon weapon, int distance)
        {
            Assert.Equal(distance, weapon.Distance);
        }

        [Theory]
        [MemberData(nameof(WeaponToMultipleBangMapping))]
        public void Only_volcanic_allows_multiple_bang(Weapon weapon, bool multipleBang)
        {
            Assert.Equal(multipleBang, weapon.MultipleBang);
        }
    }
}