namespace Bang.PlayingCards.Visitors
{
    public class IsLongTermCardTypeVisitor : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;
        public bool Visit(BarrelCardType card) => true;
        public bool Visit(CarabineCardType card) => true;
        public bool Visit(DynamiteCardType card) => true;
        public bool Visit(JailCardType card) => true;
        public bool Visit(MustangCardType card) => true;
        public bool Visit(RemingtonCardType card) => true;
        public bool Visit(SchofieldCardType card) => true;
        public bool Visit(ScopeCardType card) => true;
        public bool Visit(VolcanicCardType card) => true;
        public bool Visit(WinchesterCardType card) => true;
    }
}