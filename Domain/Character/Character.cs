namespace Domain.Character
{
    public abstract class Character : ValueObject<Character>
    {
        public abstract int LifePoints { get; }
    }
}