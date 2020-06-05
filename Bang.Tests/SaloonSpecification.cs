using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class SaloonSpecification
    {
        [Fact]
        public void After_played_saloon_card_goes_to_discard_pile()
        {
            var gameplay = InitGame();
            (Player actor, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(saloonCard);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(saloonCard);
        }
        
        [Fact]
        public void Player_discards_gatling_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(saloonCard);

            // Assert
            actor.Hand.Should().NotContain(saloonCard);
        }

        [Fact]
        public void Saloon_regains_one_life_point_to_all_alive_players()
        {
            var gameplay = InitGame();
            (Player actor, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            var otherPlayer = gameplay.Players.First(p => p != actor);

            var health = otherPlayer.LifePoints;
            otherPlayer.LoseLifePoint();

            // act
            actor.PlayCard(saloonCard);
            
            // assert
            otherPlayer.LifePoints.Should().Be(health);
        }
        
        [Fact]
        public void Saloon_regains_one_life_point_to_current_player()
        {
            var gameplay = InitGame();
            (Player actor, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            var health = actor.LifePoints;
            actor.LoseLifePoint();

            // act
            actor.PlayCard(saloonCard);
            
            // assert
            actor.LifePoints.Should().Be(health);
        }
        
        
        private (Player actor, BangGameCard saloonCard) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());

            actor.LoseLifePoint();
            var saloonCard = new SaloonCardType().HeartsAce();
            actor.AddCardToHand(saloonCard);
            
            return (actor, saloonCard);
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
    }
}