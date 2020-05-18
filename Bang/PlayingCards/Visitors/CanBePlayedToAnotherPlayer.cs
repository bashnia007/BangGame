namespace Bang.PlayingCards.Visitors
{
    public class CanBePlayedToAnotherPlayer : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(CatBalouCardType card) => true;
        public bool Visit(PanicCardType card) => true;
        public bool Visit(BangCardType card) => true;
        public bool Visit(JailCardType card) => true;
    }
}