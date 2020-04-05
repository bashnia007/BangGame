namespace Domain.Weapons
{
    public abstract class Weapon : ValueObject<Weapon>
    {
        public abstract int Distance { get; }
        public abstract bool MultipleBang { get; }

        protected override int GetHashCodeCore()
        {
            return (Distance, MultipleBang).GetHashCode();
        }
    }
}