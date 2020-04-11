using System;
using Domain.Characters.Visitors;

namespace Domain.Characters
{
    
    /// <summary>
    /// El Gringo (3 life points):
    /// each time he loses a life point due to a card played by another player, he draws a random card from the hands of that player (one card for each life point). 
    /// If that player has no more cards, too bad!, he does not draw. Note that Dynamite damages are not caused by any player.
    /// </summary>
    [Serializable]
    public class ElGringo : Character
    {
        public override string Name => CardName.ElGringo;
        public override int LifePoints => 3;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);
        

        protected override bool EqualsCore(Character other) => other is ElGringo;

        protected override int GetHashCodeCore() => typeof(ElGringo).GetHashCode();
    }
}