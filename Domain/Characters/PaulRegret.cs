﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Paul Regret (3 life points):
    /// he is considered to have a Mustang in play at all times;
    /// all other players must add 1 to the distance to him.
    /// If he has another real Mustang in play, he can count both of them, increasing all distances to him by a total of 2.
    /// </summary>
    [Serializable]
    public class PaulRegret : Character
    {
        public override string Name => CardName.PaulRegret;
        public override int LifePoints => 3;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(Character other) => other is PaulRegret;

        protected override int GetHashCodeCore() => typeof(PaulRegret).GetHashCode();
    }
}