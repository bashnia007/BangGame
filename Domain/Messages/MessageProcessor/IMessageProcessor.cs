using System.Collections.Generic;

namespace Domain.Messages
{
    public interface IMessageProcessor
    {
        List<Message> ProcessGetGamesMessage(Message message);
        List<Message> ProcessCreateGameMessage(Message message);
        List<Message> ProcessJoinGameMessage(Message message);
        List<Message> ProcessReadyToPlayMessage(Message message);
        List<Message> ProcessStartGameMessage(Message message);
        List<Message> ProcessConnectedMessage(Message message);
        List<Message> ProcessLeaveGameMessage(Message message);
    }
}
