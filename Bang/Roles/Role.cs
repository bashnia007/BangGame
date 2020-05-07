using System;
using Bang;

namespace Bang.Roles
{
    [Serializable]
    public abstract class Role : ValueObject<Role>
    {
        public abstract string Description { get; } 
    }
}