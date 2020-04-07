using Domain.Game;
using Domain.Messages;
using System;
using System.Collections.Generic;

namespace Server.Processors
{
    public class ServerMessageProcessor : IMessageProcessor
    {
        public List<Message> ProcessConnectedMessage(Message message)
        {
            var result = new List<Message>();

            var connectionMessage = (ConnectionMessage)message;
            Lobby.SetPlayerName(connectionMessage.PlayerId, connectionMessage.Name);

            return result;
        }

        public List<Message> ProcessCreateGameMessage(Message message)
        {
            var result = new List<Message>();
            var createGameMessage = (CreateGameMessage)message;

            var player = Lobby.GetPlayer(message.PlayerId);
            var game = new Game(player);
            Lobby.AddGame(game);

            createGameMessage.GameId = game.Id;
            result.Add(createGameMessage);

            return result;
        }

        public List<Message> ProcessGetGamesMessage(Message message)
        {
            var result = new List<Message>();
            var getGamesMessage = (GetGamesMessage)message;
            getGamesMessage.Games = Lobby.GetGames();
            result.Add(getGamesMessage);

            return result;
        }

        public List<Message> ProcessJoinGameMessage(Message message)
        {
            var result = new List<Message>();

            var joinGameMessage = (JoinGameMessage)message;
            var player = Lobby.GetPlayer(joinGameMessage.PlayerId);
            var game = Lobby.GetGame(joinGameMessage.GameId);

            joinGameMessage.IsJoined = game.JoinPlayer(player);

            result.Add(joinGameMessage);

            return result;
        }

        public List<Message> ProcessLeaveGameMessage(Message message)
        {
            var result = new List<Message>();

            var leaveGameMessage = (LeaveGameMessage)message;
            var player = Lobby.GetPlayer(leaveGameMessage.PlayerId);
            var game = Lobby.GetGame(leaveGameMessage.GameId);

            if (!game.KickPlayer(player))
            {
                Lobby.CloseGame(game.Id);
            }

            return result;
        }

        public List<Message> ProcessReadyToPlayMessage(Message message)
        {
            var result = new List<Message>();

            var readyToPlayMessage = (ReadyToPlayMessage)message;

            var game = Lobby.GetGame(readyToPlayMessage.GameId);
            game.SetPlayerReadyStatus(readyToPlayMessage.PlayerId, readyToPlayMessage.IsReady);

            if (game.AllPlayersAreReady())
            {
                Lobby.CloseGame(readyToPlayMessage.GameId);
                game.Initialize();
                foreach (var player in game.Players)
                {
                    var startGameMessage = new StartGameMessage(player.Role, player.PlayerTablet.Character, 
                        player.PlayerHand, game.Id, player.Id);

                    result.Add(startGameMessage);
                }
            }

            return result;
        }

        public List<Message> ProcessStartGameMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
