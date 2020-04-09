﻿using System;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Sid Ketchum (4 life points): at any time, he may discard 2 cards from his hand to regain one life point.
    /// If he is willing and able, he can use this ability more than once at a time.
    /// But remember: you cannot have more life points than your starting amount!
    /// </summary>
    [Serializable]
    public class SidKetchum : Character
    {
        public override string Name => CardName.SidKetchum;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is SidKetchum;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SidKetchum).GetHashCode();
        }
    }
}