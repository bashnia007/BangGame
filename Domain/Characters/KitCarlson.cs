﻿using System;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Kit Carlson (4 life points):
    /// during phase 1 of his turn, he looks at the top three cards of the deck:
    /// he chooses 2 to draw, and puts the other one back on the top of the deck, face down.
    /// </summary>
    [Serializable]
    public class KitCarlson : Character
    {
        public override string Name => CardName.KitCarlson;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is KitCarlson;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(KitCarlson).GetHashCode();
        }
    }
}