using Bang.Players;
using System;

namespace Server.Tests.Characters
{
    public abstract class CharactersSpecification
    {
        protected Player CreatePlayer()
        {
            string id = Guid.NewGuid().ToString();
            Lobby.AddPlayer(id);
            Lobby.SetPlayerName(id, "Dr. Who");

            return Lobby.GetPlayer(id);
        }

        protected Game CreateGame(Player player)
        {
            var game = new Game(player);
            return game;
        }

        protected Game CreateAndStartGame(int playersCount = 4)
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < playersCount - 1; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }
    }
}
