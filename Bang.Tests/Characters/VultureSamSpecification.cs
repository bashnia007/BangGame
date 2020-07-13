using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests.Characters
{
    public class VultureSamSpecification
    {
        [Fact]
        public void Vulture_Sam_starts_game_with_4_life_points()
        {
            var gamePlay = InitGameplayWithCharacter(new VultureSam());
            var (vultureSam, _) = ChoosePlayers(gamePlay);

            vultureSam.AsOutlaw(gamePlay);
            
            // Assert
            vultureSam.LifePoints.Should().Be(4);
        }
        
        [Fact]
        public void Whenever_a_character_is_eliminated_from_the_game_Sam_takes_all_active_cards_that_player_had()
        {
            var gameplay = InitGameplayWithCharacter(new VultureSam());
            var (sam, outLaw) = ChoosePlayers(gameplay);
            
            var volcanic = new VolcanicCardType().ClubsSeven();
            outLaw.PlayerTablet.PutCard(volcanic);
            
            // Act
            outLaw.LoseLifePoint(5);
            
            // Assert
            outLaw.ActiveCards.Should().BeEmpty();
            sam.Hand.Should().Contain(volcanic);
        }
        
        [Fact]
        public void Whenever_a_character_is_eliminated_from_the_game_Sam_takes_all_hand_cards_that_player_had()
        {
            var gameplay = InitGameplayWithCharacter(new VultureSam());
            var (sam, outLaw) = ChoosePlayers(gameplay);
            
            var missedCard = new MissedCardType().ClubsSeven();
            outLaw.AddCardToHand(missedCard);
            
            // Act
            outLaw.LoseLifePoint(5);
            
            // Assert
            outLaw.Hand.Should().BeEmpty();
            sam.Hand.Should().Contain(missedCard);
        }

        [Fact]
        public void
            When_Vulture_Sam_eliminates_a_Deputy_as_a_Sheriff_he_discards_all_his_cards_after_getting_the_cards_of_the_Deputy()
        {
            var gameplay = InitGameplayWithCharacter(new VultureSam());
            var (sheriffSam, deputy) = ChoosePlayers(gameplay);
            sheriffSam.AsSheriff(gameplay);
            
            var volcanic = new VolcanicCardType().HeartsAce();
            deputy.AddCardToHand(volcanic);
            deputy.AsDeputy(gameplay);
            
            // Act
            deputy.LoseLifePoint(sheriffSam, 4);
            
            // Assert
            sheriffSam.Hand.Should().BeEmpty();
            sheriffSam.ActiveCards.Should().BeEmpty();
        }

        [Fact]
        public void
            When_the_Dynamite_explodes_eliminating_a_player_Vulture_Sam_does_not_draw_the_Dynamite_along_with_all_other_cards()
        {
            var deck = new Deck<BangGameCard>();
            var gameplay = new GameplayBuilder().WithCharacter(new VultureSam()).WithDeck(deck).Build(); 
            var (vultureSam, outLaw) = ChoosePlayers(gameplay);

            outLaw.LoseLifePoint(2);
            
            var dynamite = new DynamiteCardType().HeartsAce();
            outLaw.PlayerTablet.PutCard(dynamite);
            
            gameplay.SkipTurnsUntilPlayer(outLaw);
            while (gameplay.GetNextPlayer() != outLaw)
                gameplay.SetNextPlayer();

            // Act
            deck.Put(new StagecoachCardType().ClubsSeven()); // card to explode
            gameplay.StartNextPlayerTurn();
            
            // Assert 
            vultureSam.Hand.Should().NotContain(dynamite);
        }

        private (Gameplay, Player, Player) InitGame(Gameplay gameplay)
        {
            var vultureSam = gameplay.Players.First(p => p.Character is VultureSam);
            
            var outLaw = gameplay.Players.First(p => p != vultureSam && p.Role is Outlaw);

            return (gameplay, vultureSam, outLaw);
        }

        private (Player, Player) ChoosePlayers(Gameplay gameplay)
        {
            var (_, vultureSam, sheriff) = InitGame(gameplay);

            return (vultureSam, sheriff);
        }
    }
}