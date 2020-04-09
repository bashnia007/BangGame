using Domain.Characters;
using Domain.PlayingCards;
using Domain.Roles;

using System;
using System.Collections.Generic;


namespace Domain.Players
{
    [Serializable]
    public abstract class Player
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        public List<PlayingCard> PlayerHand { get; private set; }
        public virtual bool IsReadyToPlay { get; set; }

        public Player()
        {
            PlayerHand = new List<PlayingCard>();
        }

        public void SetInfo(Role role, Character character)
        {
            Role = role;
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }
    }
}
