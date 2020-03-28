namespace Domain.Character
{
    /// <summary>
    /// Pedro Ramirez (4 life points):
    /// during phase 1 of his turn, he may choose to draw the first card from the top of the discard pile or from the deck.
    /// Then, he draws the second card from the deck.
    /// </summary>
    public class PedroRamirez : Character
    {
        public override int LifePoints => 3;
        protected override bool EqualsCore(Character other)
        {
            return other is PedroRamirez;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(PedroRamirez).GetHashCode();
        }
    }
}