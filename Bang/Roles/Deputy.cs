﻿using System;
using Bang.Roles;

namespace Bang.Roles
{
    [Serializable]
    public class Deputy : Role
    {
        public override string Description => CardName.Deputy;
        
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