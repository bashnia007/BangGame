using Bang.Weapons;

namespace Bang.PlayingCards.Visitors
{
    public class CardToWeaponVisitor : ICardTypeVisitor<Weapon>
    {
        public Weapon DefaultValue => null;
        public Weapon Visit(VolcanicCardType card) => new Volcanic();
        public Weapon Visit(SchofieldCardType card) => new Schofield();
        public Weapon Visit(CarabineCardType card) => new Carabine(); 
        public Weapon Visit(RemingtonCardType card) => new Remington(); 
        public Weapon Visit(WinchesterCardType card) => new Winchester();
    }
}