namespace Domain.Role
{
    public abstract class Role : ValueObject<Role>, IShuffledCard
    {
        public abstract string Description { get; } 
    }
}