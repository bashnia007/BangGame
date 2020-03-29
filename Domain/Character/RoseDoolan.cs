namespace Domain.Character
{
    /// <summary>
    /// Rose Doolan (4 life points):
    /// she is considered to have a Scope in play at all times;
    /// she sees the other players at a distance decreased by 1.
    /// If she has another real Scope in play, she can count both of them, reducing her distance to all other players by a total of 2
    /// </summary>
    public class RoseDoolan : Character
    {
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is RoseDoolan;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(RoseDoolan).GetHashCode();
        }
    }
}