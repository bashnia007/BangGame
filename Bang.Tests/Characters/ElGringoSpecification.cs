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
            var (elGringo, otherPlayer) = ChoosePlayers();

            var hand = elGringo.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            elGringo.Hand.Should().HaveCount(hand + 1);
        }
        
        [Fact]
        public void When_El_Gringo_loses_a_life_point_he_draws_a_card_from_hitter_hand()
        {
            var (elGringo, otherPlayer) = ChoosePlayers();

            var hand = otherPlayer.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            otherPlayer.Hand.Should().HaveCount(hand - 1);
            
        }
        
        [Fact]
        public void If_hitter_does_not_have_a_card_in_hand_El_Gringo_will_draw_nothing()
        {
            var (elGringo, otherPlayer) = ChoosePlayers();

            otherPlayer.DropAllCards();

            var handSize = elGringo.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            elGringo.Hand.Should().HaveCount(handSize);
        }
        
        [Fact]
        public void When_El_Gringo_loses_last_life_point_he_does_not_draw_card()
        {
            var (elGringo, otherPlayer) = ChoosePlayers();
            
            elGringo.LoseLifePoint(2);

            var handSize = otherPlayer.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            otherPlayer.Hand.Should().HaveCount(handSize);
        }

        [Fact]
        public void If_El_Gringo_plays_a_duel_and_lose_he_will_not_draw_a_card_from_the_player_who_won()
        {
            var (elGringo, otherPlayer) = ChoosePlayers();

            var elGringoHandSize = elGringo.Hand.Count;
            
            // Act
            elGringo.PlayCard(DuelCard(), otherPlayer);
            otherPlayer.Defense(BangCard());
            elGringo.NotDefense();
            
            // Assert
            
            // duel card is already played, so El Gringo should have 1 less card 
            elGringo.Hand.Should().HaveCount(elGringoHandSize - 1);
        }
        
        [Fact]
        public void When_El_Gringo_draws_card_from_player_hand_it_does_not_affect_discard_pile()
        {
            var (gameplay, elGringo, otherPlayer) = InitGame();

            var volcanicCard = new VolcanicCardType().ClubsSeven();
            otherPlayer.AddCardToHand(volcanicCard);
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);
            
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
            
            var charactersDeck = new Deck<Character>();
            charactersDeck.Put(new ElGringo());
            charactersDeck.Put(new KitCarlson());
            charactersDeck.Put(new SlabTheKiller());
            charactersDeck.Put(new WillyTheKid());

            var gameplay = new Game.Gameplay(charactersDeck, deck);
            gameplay.Initialize(players);

            var elGringo = players.First(p => p.Character is ElGringo);
            elGringo.SetInfo(gameplay, new Renegade(), elGringo.Character);
            
            elGringo.AddCardToHand(DuelCard());

            var otherPlayer = players.First(p => p != elGringo);
            otherPlayer.AddCardToHand(BangCard());

            return (gameplay, elGringo, otherPlayer);
        }

        private (Player, Player) ChoosePlayers()
        {
            var (_, elGringo, otherPlayer) = InitGame();

            return (elGringo, otherPlayer);
        }
    }
}