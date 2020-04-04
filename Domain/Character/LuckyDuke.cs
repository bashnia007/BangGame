namespace Domain.Character
{
    /// <summary>
    /// Lucky Duke (4 life points):
    /// each time he is required to “draw!”, he flips the top two cards from the deck, and chooses the result he prefers. Discard both cards afterwards.
    /// </summary>
    public class LuckyDuke : Character
    {
        public override string Name => CardName.LuckyDuke;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is LuckyDuke;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(LuckyDuke).GetHashCode();
        }
    }
}