using System.Collections.Generic;
using System.Linq;
using Domain.Characters;
using Domain.PlayingCards;

namespace Bang.Tests.DomainUnitTests
{
    public class TestDataGenerator
    {
        public static IEnumerable<object[]> AllCharacters()
        {
            return new[]
            {
                new object[] {new BartCassidy()},
                new object[] {new BlackJack()},
                new object[] {new CalamityJanet(),},
                new object[] {new ElGringo(),},
                new object[] {new JessyJones(),},
                new object[] {new Jourdonnais(),},
                new object[] {new KitCarlson(),},
                new object[] {new LuckyDuke(),},
                new object[] {new PaulRegret(),},
                new object[] {new PedroRamirez(),},
                new object[] {new RoseDoolan(),},
                new object[] {new SidKetchum(),},
                new object[] {new SlabTheKiller(),},
                new object[] {new SuzyLafayette(),},
                new object[] {new VultureSam(),},
                new object[] {new WillyTheKid(),},
            };
        }

        public static IEnumerable<object[]> WeaponCards =>
            new List<object[]>
            {
                new[] {new VolcanicCardType()},
                new[] {new SchofieldCardType()},
                new[] {new RemingtonCardType()},
                new[] {new CarabineCardType()},
                new[] {new WinchesterCardType()}
            };

        public static IEnumerable<object[]> LongTermCards =>
            new List<object[]>
            {
                new[] {new ScopeCardType()},
                new[] {new MustangCardType()},
                new[] {new BarrelCardType()},
                new[] {new JailCardType()},
                new[] {new DynamiteCardType()}
            }.Concat(WeaponCards);

        public static IEnumerable<object[]> PlayAndDiscardCards =>
            new List<object[]>
            {
                new[] {new BangCardType()},
                new[] {new MissedCardType()},
                new[] {new PanicCardType()},
                new[] {new CatBalouCardType()},
                new[] {new StagecoachCardType(),},
                new[] {new WellsFargoCardType(), },
                new[] {new IndiansCardType(), },
                new[] {new BeerCardType(), },
                new[] {new SaloonCardType(), },
                new[] {new GatlingCardType(), },
                new[] {new GeneralStoreCardType(), },
            };

    public static IEnumerable<object[]> DifferentWeapons
        {
            get
            {
                var weapons = new List<WeaponCardType>
                {
                    new VolcanicCardType(),
                    new SchofieldCardType(),
                    new RemingtonCardType(),
                    new CarabineCardType(),
                    new WinchesterCardType()
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
    }
}