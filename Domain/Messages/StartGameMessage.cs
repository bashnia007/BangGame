using Domain.PlayingCards;
using System.Collections.Generic;

namespace Domain.Messages
{
    public class StartGameMessage : Message
    {
        public Role.Role Role { get; }
        public Character.Character Character { get; }
        public List<PlayingCard> Hand { get; }

        public StartGameMessage(Role.Role role, Character.Character character, List<PlayingCard> hand, string gameId, string playerId)
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
