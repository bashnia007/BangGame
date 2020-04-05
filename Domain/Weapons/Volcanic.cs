namespace Domain.Weapons
{
    public class Volcanic : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Volcanic;
        }

        public override int Distance => 1;
        public override bool MultipleBang => true;
    }
}