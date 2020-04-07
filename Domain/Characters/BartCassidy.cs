namespace Domain.Characters
{
    /// <summary>
    ///  Bart Cassidy (4 life points):
    /// each time he loses a life point, he immediately draws a card from the deck.
    /// </summary>
    public class BartCassidy : Character
    {
        public override string Name => CardName.BartCassidy;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is BartCassidy;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BartCassidy).GetHashCode();
        }
    }
}