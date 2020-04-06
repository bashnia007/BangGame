using System;

namespace Domain.Role
{
    [Serializable]
    public class Renegade : Role
    {
        public override string Description => CardName.Renegade;
        
        protected override bool EqualsCore(Role other)
        {
            return other is Renegade;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(Renegade).GetHashCode();
        }
    }
}