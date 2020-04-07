using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using Domain.Characters;
using Domain.Roles;

namespace Domain.Players
{
    public abstract class Player
    {
        public string Id { get; }
        public string Name { get; set; }
        public Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        public List<PlayingCard> PlayerHand { get; private set; }

        public Player()
        {
            Id = Guid.NewGuid().ToString();
            PlayerHand = new List<PlayingCard>();
        }

        public void SetInfo(Role role, Character character)
        {
            Role = role;
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }
    }
}
