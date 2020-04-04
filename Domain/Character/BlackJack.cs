namespace Domain.Character
{
    /// <summary>
    /// Black Jack (4 life points):
    /// during phase 1 of his turn, he must show the second card he draws:
    /// if it’s Heart or Diamonds (just like a “draw!”),
    /// he draws one additional card (without revealing it).
    /// </summary>
    public class BlackJack : Character
    {
        public override string Name => CardName.BlackJack;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is BlackJack;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(BlackJack).GetHashCode();
        }
    }
}