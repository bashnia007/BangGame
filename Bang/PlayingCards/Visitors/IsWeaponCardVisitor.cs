namespace Bang.PlayingCards.Visitors
{
    public class IsWeaponCardVisitor : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(VolcanicCardType card) => true;
        public bool Visit(SchofieldCardType card) => true;
        public bool Visit(CarabineCardType card) => true;
        public bool Visit(RemingtonCardType card) => true;
        public bool Visit(WinchesterCardType card) => true;
    }
}