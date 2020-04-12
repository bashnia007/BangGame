using Domain.Game;
using Domain.Players;
using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class GameTests
    {
        [Fact]
        public void Drop_cards_removes_cards_from_hand()
        {
            var game = CreateAndStartGame();
            var player = game.Players.First();
            var cardsToDrop = player.PlayerHand.Take(2).ToList();
            game.Gameplay.DropCardsFromHand(cardsToDrop, player.Id);

            foreach(var card in cardsToDrop)
            {
                Assert.DoesNotContain(card, player.PlayerHand);
            }
        }

        [Fact]
        public void Take_cards_adds_cards_on_hand()
        {
            const int cardsToTake = 3;

            var game = CreateAndStartGame();
            var player = game.Players.First();
            int cardsOnHandBeforeTake = player.PlayerHand.Count;

            game.Gameplay.TakeCardsOnHand(cardsToTake, player.Id);

            Assert.Equal(cardsOnHandBeforeTake + cardsToTake, player.PlayerHand.Count);
        }

        #region Private methods

        private Player CreatePlayer()
        {
            var id = new Guid().ToString();
            return new PlayerOnline(id);
        }

        private List<Player> CreatePlayers(int amount)
        {
            var result = new List<Player>();

            for (int i = 0; i < amount; i++)
            {
                result.Add(CreatePlayer());
            }

            return result;
        }

        private List<PlayingCard> CreatePlayingCardsList()
        {
            return new List<PlayingCard>
            {
                new BangCard(),
                new MissedCard(),
                new DynamiteCard(),
            };
        }

        private Game CreateAndStartGame()
        {
            var player = CreatePlayer();
            var game = new Game(player);

            for (int i = 0; i < 3; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }

        #endregion
    }
}
