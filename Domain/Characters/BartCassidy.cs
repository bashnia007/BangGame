using System;
using Domain.Characters.Visitors;

namespace Domain.Characters
{
    /// <summary>
    ///  Bart Cassidy (4 life points):
    /// each time he loses a life point, he immediately draws a card from the deck.
    /// </summary>
    [Serializable]
    public class BartCassidy : Character
    {
        public override string Name => CardName.BartCassidy;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is BartCassidy;

        protected override int GetHashCodeCore() => typeof(BartCassidy).GetHashCode();
    }
}