using Domain.Weapons;

namespace Domain.PlayingCards.Visitors
{
    internal class LongTermCardTypeToWeaponMatcher : ILongTermCardTypeVisitor<Weapon>
    {
        public Weapon Visit(VolcanicCardType volcanicCardType) => new Volcanic();

        public Weapon Visit(SchofieldCardType schofieldCardType) => new Schofield();

        public Weapon Visit(RemingtonCardType remingtonCardType) => new Remington();

        public Weapon Visit(CarabineCardType carabineCardType) => new Carabine();

        public Weapon Visit(WinchesterCardType winchester) => new Winchester();

        public Weapon Visit(BarrelCardType barrelCardType) => null;
        public Weapon Visit(ScopeCardType scopeCardType) => null;
        public Weapon Visit(MustangCardType mustangCardType) => null;
        
        public Weapon Visit(JailCardType jailCardType) => null;
        public Weapon Visit(DynamiteCardType dynamiteCardType) => null;
    }
}