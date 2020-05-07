using System;
using Bang.Characters.Visitors;
using Bang;

namespace Bang.Characters
{
    [Serializable]
    public abstract class Character : ValueObject<Character>
    {
        public abstract string Name { get; } 
        public abstract int LifePoints { get; }
        internal abstract T Accept<T>(ICharacterVisitor<T> visitor);
    }
}