using System.Collections.Generic;
using Bang.Characters;
using Bang.PlayingCards;
using Bang.Roles;
using Gameplay.Characters;
using Gameplay.Roles;
using Server.Messages;

namespace Bang.Messages
{
    public class StartGameMessage : Message
    {
        public Role Role { get; }
        public Character Character { get; }
        public List<BangGameCard> Hand { get; }

        public StartGameMessage(Role role, Character character, List<BangGameCard> hand, string gameId, string playerId)
        {
            Role = role;
            Character = character;
            Hand = hand;
            GameId = gameId;
            PlayerId = playerId;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessStartGameMessage(this);
        }
    }
}
