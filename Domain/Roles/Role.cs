namespace Domain.Roles
{
    public abstract class Role : ValueObject<Role>, IShuffledCard
    {
        public abstract string Description { get; } 
    }
}