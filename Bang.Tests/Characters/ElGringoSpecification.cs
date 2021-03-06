using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

namespace Bang.Tests.Characters
{
    public class ElGringoSpecification
    {
        [Fact]
        public void El_Gringo_starts_game_with_3_life_points()
        {
            var (gameplay, elGringo, _) = InitGame();
            
            elGringo.AsRenegade(gameplay);
            // Assert
            elGringo.LifePoints.Should().Be(3);
        }
        
        [Fact]
        public void When_El_Gringo_loses_a_life_point_he_draws_a_card()
        {
            var (gameplay, elGringo, otherPlayer) = InitGame();

            var hand = elGringo.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            elGringo.Hand.Should().HaveCount(hand + 1);
        }
        
        [Fact]
        public void When_El_Gringo_loses_a_life_point_he_draws_a_card_from_hitter_hand()
        {
            var (gameplay, elGringo, otherPlayer) = InitGame();

            var hand = otherPlayer.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            otherPlayer.Hand.Should().HaveCount(hand - 1);
        }
        
        [Fact]
        public void If_hitter_does_not_have_a_card_in_hand_El_Gringo_will_draw_nothing()
        {
            var (gameplay, elGringo, otherPlayer) = InitGame();

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
            var (gameplay, elGringo, otherPlayer) = InitGame();
            elGringo.AsRenegade(gameplay);
            
            elGringo.LoseLifePoint(elGringo.MaximumLifePoints - 1);

            var handSize = otherPlayer.Hand.Count;
            
            // Act
            elGringo.LoseLifePoint(otherPlayer);

            // Assert
            otherPlayer.Hand.Should().HaveCount(handSize);
        }

        [Fact]
        public void If_El_Gringo_plays_a_duel_and_lose_he_will_not_draw_a_card_from_the_player_who_won()
        {
            var (gameplay, elGringo, otherPlayer) = InitGame();

            var elGringoHandSize = elGringo.Hand.Count;
            
            // Act
            elGringo.PlayDuel(gameplay, otherPlayer);
            otherPlayer.DefenseAgainstDuel(gameplay, BangCard());
            elGringo.LoseDuel(gameplay);
            
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
            gameplay.PeekTopCardFromDiscarded().Should().NotBe(volcanicCard);
        }
        
        private BangGameCard DuelCard() => new DuelCardType().SpadesQueen();
        private BangGameCard BangCard() => new BangCardType().HeartsAce();

        private (Gameplay, Player, Player) InitGame()
        {
            var gameplay = 
                new GameplayBuilder()
                    .WithCharacter(new ElGringo())
                    .WithCharacter(new CalamityJanet())
                    .WithoutCharacter(new VultureSam())
                    .WithoutCharacter(new SuzyLafayette())
                    .WithoutCharacter(new KitCarlson())
                    .Build();

            var elGringo = gameplay.FindPlayer(new ElGringo());
            elGringo.AddCardToHand(DuelCard());

            var otherPlayer = gameplay.Players.First(p => p.Character == new CalamityJanet());
            otherPlayer.AddCardToHand(BangCard());

            return (gameplay, elGringo, otherPlayer);
        }
    }
}