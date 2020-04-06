using System;
using System.Collections.Generic;
using Domain.Game;
using Domain.Messages;
using Domain.Players;

namespace Server.Processors
{
    public class ServerMessageProcessor : IMessageProcessor
    {
        public List<Message> ProcessCreateGameMessage(Message message)
        {
            var result = new List<Message>();
            var getGamesMessage = (GetGamesMessage)message;
            getGamesMessage.Games = GamesCollection.GetGames();
            result.Add(getGamesMessage);

            return result;
        }

        public List<Message> ProcessGetGamesMessage(Message message)
        {
            var result = new List<Message>();
            var player = new PlayerOnline();
            GamesCollection.AddGame(new Game(player));

            return result;
        }

        public List<Message> ProcessJoinGameMessage(Message message)
        {
            var result = new List<Message>();

            var joinGameMessage = (JoinGameMessage)message;
            var player = new PlayerOnline();
            GamesCollection.JoinPlayerToGame(player, joinGameMessage.GameId);

            return result;
        }

        public List<Message> ProcessReadyToPlayMessage(Message message)
        {
            var result = new List<Message>();

            var readyToPlayMessage = (ReadyToPlayMessage)message;

            var game = GamesCollection.GetGame(readyToPlayMessage.GameId);
            game.SetPlayerReadyStatus(readyToPlayMessage.PlayerId, readyToPlayMessage.IsReady);

            if (game.AllPlayersAreReady())
            {
                GamesCollection.CloseGame(readyToPlayMessage.GameId);
                var startGameMessage = new StartGameMessage();
                game.Initialize();
                result.Add(startGameMessage);
            }

            return result;
        }

        public List<Message> ProcessStartGameMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
