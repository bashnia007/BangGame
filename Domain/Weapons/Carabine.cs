using System;

namespace Domain.Weapons
{
    [Serializable]
    public class Carabine : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Carabine;
        }

        public override int Distance => 4;
        public override bool MultipleBang => false;
    }
}