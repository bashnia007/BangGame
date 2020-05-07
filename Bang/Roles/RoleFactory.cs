using System;
using Bang.Roles;

namespace Gameplay.Roles
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
