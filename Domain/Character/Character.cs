namespace Domain.Character
{
    public abstract class Character : ValueObject<Character>
    {
        public abstract string Name { get; } 
        public abstract int LifePoints { get; }
    }
}