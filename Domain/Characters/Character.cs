using Domain.Characters.Visitors;
using System;


namespace Domain.Characters
{
    [Serializable]
    public abstract class Character : ValueObject<Character>, IShuffledCard
    {
        public abstract string Name { get; } 
        public abstract int LifePoints { get; }
        internal abstract T Accept<T>(ICharacterVisitor<T> visitor);
    }
}