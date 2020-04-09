using System;

namespace Domain.Role
{
    [Serializable]
    public abstract class Role : ValueObject<Role>, IShuffledCard
    {
        public abstract string Description { get; } 
    }
}