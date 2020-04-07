namespace Domain.Characters
{
    /// <summary>
    /// Jourdonnais (4 life points):
    /// he is considered to have a Barrel in play at all times;
    /// he can “draw!” when he is the target of a BANG!, and on a Heart he is missed.
    /// If he has another real Barrel card in play, he can count both of them, giving him two chances to cancel the BANG! before playing a Missed!.
    /// /// </summary>
    public class Jourdonnais : Character
    {
        public override string Name => CardName.Jourdonnais;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is Jourdonnais;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(Jourdonnais).GetHashCode();
        }
    }
}