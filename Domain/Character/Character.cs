namespace Domain.Character
{
    public abstract class Character : ValueObject<Character>, IShuffledCard
    {
        public abstract int LifePoints { get; }
    }
}