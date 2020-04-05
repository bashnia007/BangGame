namespace Domain.Weapons
{
    public class Winchester : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Winchester;
        }

        public override int Distance => 5;
        public override bool MultipleBang => false;
    }
}