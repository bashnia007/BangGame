using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

namespace Bang.Tests.Characters
{
    public class SidKetchumSpecification
    {
        [Fact]
        public void Sid_Ketchum_starts_game_with_4_life_points()
        {
            var (gamePlay, sid) = GameplayWithSidKetchim();
            sid.AsOutlaw(gamePlay);
            
            // Assert
            sid.LifePoints.Should().Be(4);
        }

        [Fact]
        public void Sid_Ketchum_may_discard_2_cards_from_hand_to_regain_one_life_point()
        {
            // Arrange
            var (_, sid) = GameplayWithSidKetchim();
            var bangCard = new BangCardType().DiamondsTwo();
            var stagecoach = new StagecoachCardType().ClubsSix();
            
            sid.AddCardToHand(bangCard);
            sid.AddCardToHand(stagecoach);
            sid.WithOneLifePoint();

            // Act
            sid.PlayCard(bangCard, stagecoach);
            
            // Assert
            sid.LifePoints.Should().Be(2);
        }
        
        [Fact]
        public void Sid_Ketchum_discards_cards_to_use_his_ability()
        {
            // Arrange
            var (_, sid) = GameplayWithSidKetchim();
            var bangCard = new BangCardType().DiamondsTwo();
            var stagecoach = new StagecoachCardType().ClubsSix();
            
            sid.AddCardToHand(bangCard);
            sid.AddCardToHand(stagecoach);
            sid.WithOneLifePoint();

            // Act
            sid.PlayCard(bangCard, stagecoach);
            
            // Assert
            sid.Hand.Should().NotContain(bangCard);
            sid.Hand.Should().NotContain(stagecoach);
        }
        
        
        [Fact]
        public void Sid_Ketchum_can_use_his_ability_more_than_once_at_a_time()
        {
            // Arrange
            var (_, sid) = GameplayWithSidKetchim();
            var bangCard = new BangCardType().DiamondsTwo();
            var stagecoach = new StagecoachCardType().ClubsSix();
            var panicCard = new PanicCardType().HeartsJack();
            var generalStore = new GeneralStoreCardType().SpadesTen();
            
            sid.AddCardToHand(bangCard);
            sid.AddCardToHand(stagecoach);
            sid.AddCardToHand(panicCard);
            sid.AddCardToHand(generalStore);
            sid.WithOneLifePoint();

            // Act
            sid.PlayCard(bangCard, stagecoach); // first 
            sid.PlayCard(panicCard, generalStore); // second
            
            // Assert
            sid.LifePoints.Should().Be(3);
        }

        [Fact]
        public void Sid_Ketchum_can_not_use_his_ability_if_he_has_maximum_life_points()
        {
            // Arrange
            var (_, sid) = GameplayWithSidKetchim();
            var bangCard = new BangCardType().DiamondsTwo();
            var stagecoach = new StagecoachCardType().ClubsSix();
            
            sid.AddCardToHand(bangCard);
            sid.AddCardToHand(stagecoach);

            // Act
            var result = sid.PlayCard(bangCard, stagecoach);
            
            // Assert
            sid.LifePoints.Should().Be(sid.MaximumLifePoints);
            result.Should().BeOfType<NotAllowedOperation>();
        }
        
        [Fact]
        public void Sid_Ketchum_can_use_his_ability_during_other_player_turn()
        {
            // Arrange
            var (gameplay, sid) = GameplayWithSidKetchim();
            var bangCard = new BangCardType().DiamondsTwo();
            var stagecoach = new StagecoachCardType().ClubsSix();
            
            sid.AddCardToHand(bangCard);
            sid.AddCardToHand(stagecoach);
            sid.WithOneLifePoint();

            var playerTurn = gameplay.PlayerTurn;
            if (playerTurn == sid)
                sid.EndTurn();
            playerTurn = gameplay.PlayerTurn;

            // Act
            sid.PlayCard(bangCard, stagecoach);
            
            // Assert
            sid.LifePoints.Should().Be(2);
        }
        
        private (Gameplay, Player) GameplayWithSidKetchim()
        {
            var sid = new SidKetchum();
            var gamePlay = TestUtils.InitGameplayWithCharacter(sid);

            return (gamePlay, gamePlay.FindPlayer(sid));
        }
    }
}