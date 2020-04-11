﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Rose Doolan (4 life points):
    /// she is considered to have a Scope in play at all times;
    /// she sees the other players at a distance decreased by 1.
    /// If she has another real Scope in play, she can count both of them, reducing her distance to all other players by a total of 2
    /// </summary>
    [Serializable]
    public class RoseDoolan : Character
    {
        public override string Name => CardName.RoseDoolan;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is RoseDoolan;

        protected override int GetHashCodeCore() => typeof(RoseDoolan).GetHashCode();
    }
}