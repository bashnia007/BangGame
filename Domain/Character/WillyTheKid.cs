﻿namespace Domain.Character
{
    /// <summary>
    /// Willy the Kid (4 life points): he can play any number of BANG! cards during his turn
    /// </summary>
    public class WillyTheKid : Character
    {
        public override int LifePoints => 4;
        protected override bool EqualsCore(Character other)
        {
            return other is WillyTheKid;
        }

        protected override int GetHashCodeCore()
        {
            return typeof(WillyTheKid).GetHashCode();
        }
    }
}