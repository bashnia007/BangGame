namespace Domain.Character
{
    /// <summary>
    /// Calamity Janet (4 life points):
    /// she can use BANG! cards as Missed! cards and vice versa.
    /// If she plays a Missed! as a BANG!, she cannot play another BANG! card that turn (unless she has a Volcanic in play).
    /// </summary>
    public class CalamityJanet : Character
    {
        public override string Name => CardName.CalamityJanet;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is CalamityJanet;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(CalamityJanet).GetHashCode();
        }
    }
}