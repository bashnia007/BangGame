namespace Domain.Character
{
    public abstract class Character : ValueObject<Character>, IShuffledCard
    {
        public abstract string Name { get; } 
        public abstract int LifePoints { get; }
    }
}