using System.Collections.Generic;
using System.Linq;
using Domain.Character;
using Domain.PlayingCards;

namespace Bang.Tests.DomainUnitTests
{
    public class TestDataGenerator
    {
        public static IEnumerable<object[]> AllCharacters()
        {
            return new []
            {
                new object [] {new BartCassidy()},
                new object [] {new BlackJack()},
                new object [] {new CalamityJanet(),},
                new object [] {new ElGringo(),},
                new object [] {new JessyJones(),},
                new object [] {new Jourdonnais(),},
                new object [] {new KitCarlson(),},
                new object [] {new LuckyDuke(),},
                new object [] {new PaulRegret(),},
                new object [] {new PedroRamirez(),},
                new object [] {new RoseDoolan(),},
                new object [] {new SidKetchum(),},
                new object [] {new SlabTheKiller(),},
                new object [] {new SuzyLafayette(),},
                new object [] {new VultureSam(),},
                new object [] {new WillyTheKid(),},
            };
        }
        
        public static IEnumerable<object[]> WeaponCards =>
            new List<object[]>
            {
                new []{new VolcanicCard()},
                new []{new SchofieldCard()},
                new []{new RemingtonCard()},
                new []{new CarabineCard()},
                new []{new WinchesterCard()}
            };
        
        public static IEnumerable<object[]> LongTermCards => 
            new List<object[]>
            {
                new[] {new ScopeCard()},
                new[] {new MustangCard()},
                new[] {new BarrelCard()},
                new[] {new JailCard()},
                new[] {new DynamiteCard()}
            }.Concat(WeaponCards);
        
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
    }
}