﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Lucky Duke (4 life points):
    /// each time he is required to “draw!”, he flips the top two cards from the deck, and chooses the result he prefers. Discard both cards afterwards.
    /// </summary>
    [Serializable]
    public class LuckyDuke : Character
    {
        public override string Name => CardName.LuckyDuke;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is LuckyDuke;

        protected override int GetHashCodeCore()
        {
            return typeof(LuckyDuke).GetHashCode();
        }
    }
}