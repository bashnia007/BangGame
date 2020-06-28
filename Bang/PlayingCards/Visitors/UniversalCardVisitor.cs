namespace Bang.PlayingCards.Visitors
{
    public class UniversalCardVisitor : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(BangCardType card) => true;
        public bool Visit(MissedCardType card) => true;
    }
}
