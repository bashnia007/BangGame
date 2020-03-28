namespace Domain.Role
{
    public class Renegade : Role
    {
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