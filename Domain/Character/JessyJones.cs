namespace Domain.Character
{
    /// <summary>
    /// Jesse Jones (4 life points):
    /// during phase 1 of his turn, he may choose to draw the first card from the deck, or randomly from the hand of any other player.
    /// Then he draws the second card from the deck.
    /// </summary>
    public class JessyJones : Character
    {
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is JessyJones;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(JessyJones).GetHashCode();
        }
    }
}