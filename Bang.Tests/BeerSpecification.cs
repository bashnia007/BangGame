using System;
using System.Linq;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class BeerSpecification
    {
        [Fact]
        public void Beer_card_can_not_be_used_to_help_other_players()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);

            var otherPlayer = gameplay.Players.First(p => p != actor);
            
            // act
            Action action = () => actor.PlayCard(beerCard, otherPlayer);
            
            // assert
            action.Should().Throw<InvalidOperationException>().WithMessage("* can not be played to another player!");
        }
        
        [Fact]
        public void After_played_beer_card_goes_to_discard_pile()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);
            
            // act
            actor.PlayCard(beerCard);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(beerCard);
        }
         
        [Fact]
        public void Player_discards_beer_card_after_it_played()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(beerCard);
            
            // Assert
            actor.Hand.Should().NotContain(beerCard);
        }
        
        [Fact]
        public void Beer_card_regains_one_life_point()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);

            int healthBefore = actor.LifePoints;
            // act
            actor.PlayCard(beerCard);
            
            // Assert
            actor.LifePoints.Should().Be(healthBefore + 1);
        }
        
        [Fact]
        public void Player_cannot_gain_more_life_points_than_his_starting_amount()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);

            int fullHealth = actor.PlayerTablet.MaximumHealth;
            actor.PlayerTablet.Health = fullHealth;
            
            // act
            actor.PlayCard(beerCard);
            
            // Assert
            actor.LifePoints.Should().Be(fullHealth);
        }
        
        [Fact]
        public void Beer_has_no_effect_if_there_are_only_two_players_left()
        {
            var gameplay = InitGameplay();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);
            Player opponent = gameplay.Players.First();

            foreach (var player in gameplay.Players)
            {
                if (player != actor && player != opponent)
                    KillPlayer(player);
            }

            var lifePointsBefore = actor.LifePoints;

            // act
            actor.PlayCard(beerCard);
            
            // Assert
            actor.LifePoints.Should().Be(lifePointsBefore);
        }

        private (Player actor, BangGameCard beerCard) ChoosePlayer(Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.LoseLifePoint();
            
            var beerCard = new BeerCardType().HeartsAce();
            actor.AddCardToHand(beerCard);
            
            return (actor, beerCard);
        }

        private void KillPlayer(Player player)
        {
            player.LoseLifePoint(player.MaximumLifePoints);
        }
    }
}