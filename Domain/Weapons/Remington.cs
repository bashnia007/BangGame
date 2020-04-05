namespace Domain.Weapons
{
    public class Remington : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Remington;
        }

        public override int Distance => 3;
        public override bool MultipleBang => false;
    }
}