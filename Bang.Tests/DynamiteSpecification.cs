using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class DynamiteSpecification
    {
        #region Tests

        [Fact]
        public void Player_discards_dynamite_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, _) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(DynamiteCard());

            // Assert
            actor.Hand.Should().NotContain(DynamiteCard());
        }

        [Fact]
        public void Player_has_dynamite_card_on_his_tablet_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, _) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(DynamiteCard());

            // Assert
            actor.PlayerTablet.ActiveCards.Should().Contain(DynamiteCard());
        }

        #endregion

        #region Private methods

        private Game.Gameplay InitGame() => InitGame(BangGameDeck());

        private Game.Gameplay InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }

            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);
            return gameplay;
        }

        private (Player, Player) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(DynamiteCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, victim.Role, new PedroRamirez());

            return (actor, victim);
        }

        private BangGameCard DynamiteCard() => new DynamiteCardType().SpadesQueen();

        #endregion
    }
}
