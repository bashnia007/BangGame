using Domain.Game;
using Domain.Messages;
using System;
using System.Collections.Generic;

namespace Server.Processors
{
    public class ServerMessageProcessor : IMessageProcessor
    {
        public List<Message> ProcessConnectedMessage(ConnectionMessage message)
        {
            var result = new List<Message>();
            
            Lobby.SetPlayerName(message.PlayerId, message.Name);

            return result;
        }

        public List<Message> ProcessCreateGameMessage(CreateGameMessage message)
        {
            var result = new List<Message>();

            var player = Lobby.GetPlayer(message.PlayerId);
            var game = new Game(player);
            Lobby.AddGame(game);

            message.GameId = game.Id;
            result.Add(message);

            return result;
        }

        public List<Message> ProcessGetGamesMessage(GetGamesMessage message)
        {
            var result = new List<Message>();
            message.Games = Lobby.GetGames();
            result.Add(message);

            return result;
        }

        public List<Message> ProcessJoinGameMessage(JoinGameMessage message)
        {
            var result = new List<Message>();

            var player = Lobby.GetPlayer(message.PlayerId);
            var game = Lobby.GetGame(message.GameId);

            message.IsJoined = game.JoinPlayer(player);

            result.Add(message);

            return result;
        }

        public List<Message> ProcessLeaveGameMessage(LeaveGameMessage message)
        {
            var result = new List<Message>();
            
            var player = Lobby.GetPlayer(message.PlayerId);
            var game = Lobby.GetGame(message.GameId);

            message.IsSuccess = game.KickPlayer(player);
            result.Add(message);

            if (game.GetPlayersAmount() == 0)
            {
                Lobby.CloseGame(game.Id);
            }

            return result;
        }

        public List<Message> ProcessReadyToPlayMessage(ReadyToPlayMessage message)
        {
            var result = new List<Message>();

            var game = Lobby.GetGame(message.GameId);
            game.SetPlayerReadyStatus(message.PlayerId, message.IsReady);

            if (game.AllPlayersAreReady())
            {
                Lobby.CloseGame(message.GameId);
                game.Start();
                foreach (var player in game.Players)
                {
                    var startGameMessage = new StartGameMessage(player.Role, player.PlayerTablet.Character, 
                        player.PlayerHand, game.Id, player.Id);

                    result.Add(startGameMessage);
                }
            }

            return result;
        }

        public List<Message> ProcessStartGameMessage(StartGameMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
