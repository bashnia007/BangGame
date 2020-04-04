using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Players
{
    /// <summary>
    /// Describes all VISIBLE FOR EVERYONE information about player
    /// </summary>
    public class PlayerTablet
    {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public WeaponCard WeaponCard { get; set; }
        public Character.Character Character { get; }
        public bool IsSheriff { get; }
        public List<LongTermFeatureCard> LongTermFeatureCards { get; set; }

        public PlayerTablet(Character.Character character, bool isSheriff)
        {
            Character = character;
            IsSheriff = isSheriff;
            Health = isSheriff ? Character.LifePoints + 1 : Character.LifePoints;
        }
    }
}
