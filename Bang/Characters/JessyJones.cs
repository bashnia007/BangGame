using System;
using Bang.Characters.Visitors;
using Bang.Characters;

namespace Bang.Characters
{
    
    /// <summary>
    /// Jesse Jones (4 life points):
    /// during phase 1 of his turn, he may choose to draw the first card from the deck, or randomly from the hand of any other player.
    /// Then he draws the second card from the deck.
    /// </summary>
    [Serializable]
    public class JessyJones : Character
    {
        public override string Name => CardName.JessyJones;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(Character other) => other is JessyJones;

        protected override int GetHashCodeCore() => typeof(JessyJones).GetHashCode();
    }
}