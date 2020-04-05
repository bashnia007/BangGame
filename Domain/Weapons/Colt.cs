namespace Domain.Weapons
{
    public class Colt : Weapon
    {
        protected override bool EqualsCore(Weapon other) => other is Colt;

        public override int Distance => 1;
        public override bool MultipleBang => false;
    }
}