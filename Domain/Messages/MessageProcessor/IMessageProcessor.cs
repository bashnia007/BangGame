using System.Collections.Generic;

namespace Domain.Messages
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

        List<Message> ProcessTakeCardsMessage(TakeCardsMessage message);
        List<Message> ProcessDropCardsMessage(DropCardsMessage message);

        List<Message> ProcessLongTermFeatureCardMessage(LongTermFeatureCardMessage message);
        List<Message> ProcessChangeWeaponMessage(ChangeWeaponMessage message);
        List<Message> ProcessReplenishHandMessage(ReplenishHandCardMessage message);
    }
}
