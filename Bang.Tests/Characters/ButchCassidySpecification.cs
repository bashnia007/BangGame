using System;
using System.Collections.Generic;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class ButchCassidySpecification
    {
        [Fact]
        public void Butch_Cassidy_starts_game_with_4_life_points()
        {
            var gamePlay = InitGame();
            var butch = SetCharacter(gamePlay, new BartCassidy());

            butch.LifePoints.Should().Be(4);
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void When_Butch_Cassidy_loses_a_life_point_he_draws_a_card(int lifePoints)
        {
            var gamePlay = InitGame();
            var butch = SetCharacter(gamePlay, new BartCassidy());

            var hand = butch.Hand.Count;
            
            // Act
            butch.LoseLifePoint(lifePoints);

            butch.Hand.Count.Should().Be(hand + lifePoints);
        }
        
        [Fact]
        public void When_Butch_Cassidy_loses_a_life_point_he_draws_a_card_from_the_deck()
        {
            var gamePlay = InitGame();
            var butch = SetCharacter(gamePlay, new BartCassidy());

            var cardFromDeck = gamePlay.GetTopCardFromDeck();
            
            // Act
            butch.LoseLifePoint();

            butch.Hand.Should().Contain(cardFromDeck);
        }
        
        [Fact]
        public void When_Butch_Cassidy_loses_last_life_point_he_does_not_draw_card()
        {
            var gamePlay = InitGame();
            var butch = SetCharacter(gamePlay, new BartCassidy());

            butch.LoseLifePoint(3);
            
            var topDeckCard = gamePlay.GetTopCardFromDeck();
            
            // Act
            butch.LoseLifePoint();

            gamePlay.GetTopCardFromDeck().Should().Be(topDeckCard);
        }
        
        [Fact]
        public void Butch_Cassidy_draws_card_only_if_he_is_still_alive()
        {
            var gamePlay = InitGame();
            var butch = SetCharacter(gamePlay, new BartCassidy());

            var topDeckCard = gamePlay.GetTopCardFromDeck();
            
            // Act
            butch.LoseLifePoint(4);

            gamePlay.GetTopCardFromDeck().Should().Be(topDeckCard);
        }
        
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
            actor.SetInfo(gameplay, new Outlaw(), character);

            return actor;
        }
    }
}