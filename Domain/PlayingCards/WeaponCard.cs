namespace Domain.PlayingCards
{
    public abstract class WeaponCard : LongTermFeatureCard
    {
        public abstract int Distance { get; }
        public abstract bool MultipleBang { get; }

        protected override int GetHashCodeCore()
        {
            return (Distance, MultipleBang).GetHashCode();
        }
    }
}