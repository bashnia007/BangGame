using Domain.Weapons;

namespace Domain.PlayingCards.Visitors
{
    internal class LongTermCardToWeaponMatcher : ILongTermCardVisitor<Weapon>
    {
        public Weapon Visit(VolcanicCard volcanicCard) => new Volcanic();

        public Weapon Visit(SchofieldCard schofieldCard) => new Schofield();

        public Weapon Visit(RemingtonCard remingtonCard) => new Remington();

        public Weapon Visit(CarabineCard carabineCard) => new Carabine();

        public Weapon Visit(WinchesterCard winchester) => new Winchester();

        public Weapon Visit(BarrelCard barrelCard) => null;
        public Weapon Visit(ScopeCard scopeCard) => null;
        public Weapon Visit(MustangCard mustangCard) => null;
        
        public Weapon Visit(JailCard jailCard) => null;
        public Weapon Visit(DynamiteCard dynamiteCard) => null;
    }
}