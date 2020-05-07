using System;
using Bang.Characters.Visitors;
using Bang;

namespace Bang.Characters
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
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is SidKetchum;

        protected override int GetHashCodeCore() => typeof(SidKetchum).GetHashCode();
    }
}