using System;

namespace Domain.Role
{
    [Serializable]
    public class Outlaw : Role
    {
        public override string Description => CardName.Outlaw;
        
        protected override bool EqualsCore(Role other)
        {
            return other is Outlaw;
        }

        protected override int GetHashCodeCore()
        {
            return (typeof(Outlaw)).GetHashCode();
        }
    }
}