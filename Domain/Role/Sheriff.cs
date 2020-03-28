namespace Domain.Role
{
    public class Sheriff : Role
    {
        protected override bool EqualsCore(Role other)
        {
            return other is Sheriff;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(Sheriff).GetHashCode();
        }
    }
}