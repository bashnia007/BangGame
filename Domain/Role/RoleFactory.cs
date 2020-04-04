namespace Domain.Role
{
    public class RoleFactory<T> where T : Role, new()
    {
        public T GetInstance()
        {
            return new T();
        }
    }
}
