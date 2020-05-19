using System;

namespace Bang.Roles
{
    [Serializable]
    public class Sheriff : Role
    {
        public override string Description => CardName.Sheriff;
        
        protected override bool EqualsCore(Role other)
        {
            return other is Sheriff;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(Sheriff).GetHashCode();
        }
    }
}