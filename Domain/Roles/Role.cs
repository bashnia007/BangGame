using System;

namespace Domain.Roles
{
    [Serializable]
    public abstract class Role : ValueObject<Role>, IShuffledCard
    {
        public abstract string Description { get; } 
    }
}