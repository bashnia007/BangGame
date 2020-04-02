namespace Domain.Character
{
    /// <summary>
    /// Suzy Lafayette (4 life points):
    /// as soon as she has no cards in her hand, she draws a card from the draw pile.
    /// </summary>
    public class SuzyLafayette : Character
    {
        public override string Name => CardName.SuzyLafayette;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is SuzyLafayette;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SuzyLafayette).GetHashCode();
        }
    }
}