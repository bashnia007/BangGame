using Domain.PlayingCards;
using System;
using System.Collections.Generic;

namespace Domain.Players
{
    public abstract class Player
    {
        public string Id { get; }
        public string Name { get; set; }
        public Role.Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        public List<PlayingCard> PlayerHand { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid().ToString();
            PlayerHand = new List<PlayingCard>();
        }

        public void SetInfo(Role.Role role, Character.Character character)
        {
            Role = role;
            PlayerTablet = new PlayerTablet(character, role is Role.Sheriff);
        }
    }
}
