﻿using System;
using Bang.Characters.Visitors;
using Bang.Characters;

namespace Bang.Characters
{
    
    /// <summary>
    /// Vulture Sam (4 life points):
    /// whenever a character is eliminated from the game,
    /// Sam takes all the cards that player had in his hand and in play, and adds them to his hand
    /// </summary>
    [Serializable]
    public class VultureSam : Character
    {
        public override string Name => CardName.VultureSam;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is VultureSam;

        protected override int GetHashCodeCore() => typeof(VultureSam).GetHashCode();
    }
}