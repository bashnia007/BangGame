using System;
using Bang.Characters.Visitors;

namespace Bang.Characters
{
    /// <summary>
    /// Black Jack (4 life points):
    /// during phase 1 of his turn, he must show the second card he draws:
    /// if it’s Heart or Diamonds (just like a “draw!”),
    /// he draws one additional card (without revealing it).
    /// </summary>
    [Serializable]
    public class BlackJack : Character
    {
        public override string Name => CardName.BlackJack;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(Character other) => other is BlackJack;

        protected override int GetHashCodeCore() => typeof(BlackJack).GetHashCode();
    }
}