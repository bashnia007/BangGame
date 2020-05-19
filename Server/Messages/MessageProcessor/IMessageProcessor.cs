using System.Collections.Generic;
using Server.Messages;

namespace Bang.Messages
{
    public interface IMessageProcessor
    {
        List<Message> ProcessGetGamesMessage(GetGamesMessage message);
        List<Message> ProcessCreateGameMessage(CreateGameMessage message);
        List<Message> ProcessJoinGameMessage(JoinGameMessage message);
        List<Message> ProcessReadyToPlayMessage(ReadyToPlayMessage message);
        List<Message> ProcessStartGameMessage(StartGameMessage message);
        List<Message> ProcessConnectedMessage(ConnectionMessage message);
        List<Message> ProcessLeaveGameMessage(LeaveGameMessage message);
        List<Message> ProcessGameEventMessage(BangEventMessage message);
    }
}
