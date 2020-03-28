namespace Domain.Role
{
    public class Outlaw : Role
    {
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