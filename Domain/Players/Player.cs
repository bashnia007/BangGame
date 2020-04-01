using Domain.PlayingCards;
using System;
using System.Collections.Generic;

namespace Domain.Players
{
    public abstract class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Role.Role Role { get; set; }
        public PlayerTablet PlayerTablet { get; set; }
        public List<PlayingCard> PlayerHand { get; set; }

        public Player()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
