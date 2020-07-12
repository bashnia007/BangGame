using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests.Characters
{
    public class JourdonnaisSpecification
    {
        #region Tests

        [Fact]
        public void Jourdonnais_starts_game_with_4_life_points()
        {
            var gamePlay = InitGame();
            if (gamePlay.PlayerTurn.Role is Sheriff)
                gamePlay.SetNextPlayer();
            var jourdonnais = SetCharacter(gamePlay, new Jourdonnais());

            jourdonnais.LifePoints.Should().Be(4);
        }

        [Fact]
        public void Jourdonnais_checks_default_barrel_when_attacked()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());

            var gamePlay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<Done>();
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Jourdonnais_can_use_second_barrel_if_default_barrel_did_not_help()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new MissedCardType().ClubsSeven());
            deck.Put(new StagecoachCardType().HeartsAce());

            var gamePlay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var barrelCard = new BarrelCardType().SpadesQueen();
            victim.AddCardToHand(barrelCard);
            victim.PlayCard(barrelCard);

            var healthBefore = victim.PlayerTablet.Health;

            // Act
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<Done>();
            victim.LifePoints.Should().Be(healthBefore);
        }

        #endregion

        #region Private methods

        private BangGameCard BangCard() => new BangCardType().SpadesQueen();

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

        private Player SetCharacter(Game.Gameplay gameplay, Character character)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, character);

            return actor;
        }

        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            actor.AddCardToHand(BangCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new Jourdonnais());

            return (actor, victim);
        }

        #endregion
    }
}
