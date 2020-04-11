﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Calamity Janet (4 life points):
    /// she can use BANG! cards as Missed! cards and vice versa.
    /// If she plays a Missed! as a BANG!, she cannot play another BANG! card that turn (unless she has a Volcanic in play).
    /// </summary>
    [Serializable]
    public class CalamityJanet : Character
    {
        public override string Name => CardName.CalamityJanet;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(Character other) => other is CalamityJanet;

        protected override int GetHashCodeCore() => typeof(CalamityJanet).GetHashCode();
    }
}