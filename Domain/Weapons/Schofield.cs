using System;

namespace Domain.Weapons
{
    [Serializable]
    public class Schofield : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Schofield;
        }

        public override int Distance => 2;
        public override bool MultipleBang => false;
    }
}