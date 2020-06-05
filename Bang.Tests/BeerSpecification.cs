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
    public class BeerSpecification
    {
        [Fact]
        public void Beer_card_can_not_be_used_to_help_other_players()
        {
            var gameplay = InitGame();
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
            var gameplay = InitGame();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);
            
            // act
            actor.PlayCard(beerCard);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(beerCard);
        }
         
        [Fact]
        public void Player_discards_beer_card_after_it_played()
        {
            var gameplay = InitGame();
            (Player actor, BangGameCard beerCard) = ChoosePlayer(gameplay);

            // act
            actor.PlayCard(beerCard);
            
            // Assert
            actor.Hand.Should().NotContain(beerCard);
        }
        
        [Fact]
        public void Beer_card_regains_one_life_point()
        {
            var gameplay = InitGame();
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
            var gameplay = InitGame();
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
            var gameplay = InitGame();
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
        
        private (Player actor, BangGameCard beerCard) ChoosePlayer(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());

            actor.LoseLifePoint();
            var beerCard = new BeerCardType().HeartsAce();
            actor.AddCardToHand(beerCard);
            
            return (actor, beerCard);
        }

        private void KillPlayer(Player player)
        {
            while (player.LifePoints > 0)
            {
                player.LoseLifePoint();
            }
        }
    }
}