﻿using System;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Slab the Killer (4 life points):
    /// players trying to cancel his BANG! cards need to play 2 Missed!.
    /// The Barrel effect, if successfully used, only counts as one Missed!.
    /// </summary>
    [Serializable]
    public class SlabTheKiller : Character
    {
        public override string Name => CardName.SlabTheKiller;
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is SlabTheKiller;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(SlabTheKiller).GetHashCode();
        }
    }
}