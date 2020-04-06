using System;

namespace Domain.Role
{
    [Serializable]
    public class RoleFactory<T> where T : Role, new()
    {
        public T GetInstance()
        {
            return new T();
        }
    }
}
