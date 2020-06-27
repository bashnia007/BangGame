using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ElGringoSpecification
    {
        [Fact]
        public void El_Gringo_starts_game_with_3_life_points()
        {
            var (elGringo, _) = ChoosePlayers();

            // Assert
            elGringo.LifePoints.Should().Be(3);
        }
        
        [Fact]
        public void When_El_Gringo_loses_a_life_point_he_draws_a_card()
        {
            var (elGringo, sheriff) = ChoosePlayers();

            var hand = elGringo.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(sheriff);

            // Assert
            elGringo.Hand.Should().HaveCount(hand + 1);
        }
        
        [Fact]
        public void When_El_Gringo_loses_a_life_point_he_draws_a_card_from_hitter_card()
        {
            var (elGringo, sheriff) = ChoosePlayers();

            var hand = sheriff.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(sheriff);

            // Assert
            sheriff.Hand.Should().HaveCount(hand - 1);
            
        }
        
        [Fact]
        public void If_hitter_does_not_have_a_card_in_hand_El_Gringo_will_draw_nothing()
        {
            var (elGringo, sheriff) = ChoosePlayers();

            sheriff.DropAllCards();

            var handSize = elGringo.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(sheriff);

            // Assert
            elGringo.Hand.Should().HaveCount(handSize);
        }
        
        [Fact]
        public void When_El_Gringo_loses_last_life_point_he_does_not_draw_card()
        {
            var (elGringo, sheriff) = ChoosePlayers();
            
            elGringo.LoseLifePoint(2);

            var handSize = sheriff.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(sheriff);

            // Assert
            sheriff.Hand.Should().HaveCount(handSize);
        }

        [Fact]
        public void If_El_Gringo_plays_a_duel_and_lose_he_will_not_draw_a_card_from_the_player_who_won()
        {
            var (elGringo, sheriff) = ChoosePlayers();

            var elGringoHandSize = elGringo.Hand.Count;
            
            // Act
            elGringo.PlayCard(DuelCard(), sheriff);
            sheriff.Defense(BangCard());
            elGringo.NotDefense();
            
            // Assert
            
            // duel card is already played, so El Gringo should have 1 less card 
            elGringo.Hand.Should().HaveCount(elGringoHandSize - 1);
        }
        
        [Fact]
        public void When_El_Gringo_draws_card_from_player_hand_it_does_not_affect_discard_pile()
        {
            var (gameplay, elGringo, sheriff) = InitGame();

            var volcanicCard = new VolcanicCardType().ClubsSeven();
            sheriff.AddCardToHand(volcanicCard);
            
            // Act
            elGringo.LoseLifePoint(sheriff);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().NotBe(volcanicCard);
        }
        
        private (Game.Gameplay, Player, Player) InitGame() => InitGame(BangGameDeck());
        
        private BangGameCard DuelCard() => new DuelCardType().SpadesQueen();
        private BangGameCard BangCard() => new BangCardType().HeartsAce();

        private (Game.Gameplay, Player, Player) InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }

            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);

            var elGringo = players.First(p => p.Role is Renegade);
            elGringo.SetInfo(gameplay, new Renegade(), new ElGringo());
            
            elGringo.AddCardToHand(DuelCard());

            var sheriff = players.First(p => p.Role is Sheriff);
            sheriff.AddCardToHand(BangCard());

            return (gameplay, elGringo, sheriff);
        }

        private (Player, Player) ChoosePlayers()
        {
            var (_, elGringo, sheriff) = InitGame();

            return (elGringo, sheriff);
        }
    }
}