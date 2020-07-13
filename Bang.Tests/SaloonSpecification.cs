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
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class SaloonSpecification
    {
        [Fact]
        public void After_played_saloon_card_goes_to_discard_pile()
        {
            var gameplay = InitGameplay();
            (Player player, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            // act
            player.PlayCard(saloonCard);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(saloonCard);
        }
        
        [Fact]
        public void Player_discards_saloon_card_after_it_played()
        {
            var gameplay = InitGameplay();
            (Player player, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            // act
            player.PlayCard(saloonCard);

            // Assert
            player.Hand.Should().NotContain(saloonCard);
        }

        [Fact]
        public void Saloon_regains_one_life_point_to_all_alive_players_with_no_full_health()
        {
            var gameplay = InitGameplay();
            (Player player, BangGameCard saloonCard) = ChoosePlayer(gameplay);

            var otherPlayer = gameplay.Players.First(p => p != player);

            var playerHealth = player.LifePoints;
            player.LoseLifePoint();
            
            var otherPlayerHealth = otherPlayer.LifePoints;
            otherPlayer.LoseLifePoint();

            // act
            player.PlayCard(saloonCard);
            
            // assert
            player.LifePoints.Should().Be(playerHealth);
            otherPlayer.LifePoints.Should().Be(otherPlayerHealth);
        }
        
        private (Player actor, BangGameCard saloonCard) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;

            actor.LoseLifePoint();
            var saloonCard = new SaloonCardType().HeartsAce();
            actor.AddCardToHand(saloonCard);
            
            return (actor, saloonCard);
        }
    }
}