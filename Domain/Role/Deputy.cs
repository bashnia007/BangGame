namespace Domain.Role
{
    public class Deputy : Role
    {
        protected override bool EqualsCore(Role other)
        {
            return other is Deputy;
        }

        protected override int GetHashCodeCore()
        {
            return (typeof(Deputy)).GetHashCode();
        }
    }
}