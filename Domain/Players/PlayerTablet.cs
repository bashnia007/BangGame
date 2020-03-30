using Domain.PlayingCards;

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
        public Character.Character Character { get; set; }
        public bool IsSheriff { get; set; }
    }
}
