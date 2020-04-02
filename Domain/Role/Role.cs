namespace Domain.Role
{
    public abstract class Role : ValueObject<Role>
    {
        public abstract string Description { get; } 
    }
}