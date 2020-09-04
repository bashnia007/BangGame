using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;

namespace Bang.Tests
{
    public class GameoverSpecification
    {
        [Fact]
        public void When_all_the_Outlaws_and_the_Renegade_are_killed_the_Sheriff_and_his_Deputies_win()
        {
            var gameplay = GameplayWithDeputiesAndOutlawsEliminated();

            var expectedWinners = gameplay.Players.Where(p => p.Role is Sheriff || p.Role is Deputy);

            var sheriff = gameplay.FindPlayer(new Sheriff());
            var renegade = gameplay.FindPlayer(new Renegade());
            
            renegade.WithOneLifePoint();
            
            sheriff.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(sheriff);
            
            // Act 
            sheriff.PlayDuel(gameplay, renegade);
            var response = renegade.LoseDuel(gameplay);
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();

            var specificResponse = ((GameOverResponse) response);
            specificResponse.Team.Should().Be(Team.Sheriff);
            specificResponse.Winners.Should().BeEquivalentTo(expectedWinners); 
        }
        
        [Fact]
        public void If_the_Renegade_is_the_only_one_alive_then_he_wins()
        {
            var gameplay = GameplayWithDeputiesAndOutlawsEliminated();

            var sheriff = gameplay.FindPlayer(new Sheriff());
            var renegade = gameplay.FindPlayer(new Renegade());
            
            sheriff.WithOneLifePoint();
            
            renegade.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(renegade);
            
            // Act 
            renegade.PlayDuel(gameplay, sheriff);
            var response = sheriff.LoseDuel(gameplay);
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = ((GameOverResponse) response);
            specificResponse.Team.Should().Be(Team.Renegade);
            specificResponse.Winners.Should().OnlyContain(p => p == renegade); 
        }
        
        [Fact]
        public void If_Sheriff_killed_and_non_Renegade_player_is_alive_then_Outlaws_win()
        {
            var gameplay = GameplayWithDeputiesEliminated();

            var expectedWinners = gameplay.Players.Where(p => p.Role is Outlaw);

            var sheriff = gameplay.FindPlayer(new Sheriff());
            var outlaw = gameplay.FindPlayer(new Outlaw());
            
            sheriff.WithOneLifePoint();
            
            outlaw.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(outlaw);
            
            // Act 
            outlaw.PlayDuel(gameplay, sheriff);
            var response = sheriff.LoseDuel(gameplay);
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response;
            specificResponse.Team.Should().Be(Team.Outlaws);
            specificResponse.Winners.Should().BeEquivalentTo(expectedWinners); 
        }
        
        [Fact]
        public void When_Renegade_kills_last_Deputy_and_Sheriff_by_indians_then_he_wins()
        {
            var gameplay = GameplayWithOutlawsEliminated(5);

            var sheriff = gameplay.FindPlayer(new Sheriff());
            var renegade = gameplay.FindPlayer(new Renegade());
            var deputy = gameplay.FindPlayer(new Deputy());
            
            sheriff.WithOneLifePoint();
            deputy.WithOneLifePoint();
            
            renegade.AddCardToHand(IndiansCard());
            gameplay.SetTurnToPlayer(renegade);
            
            // Act 
            renegade.PlayIndians(gameplay);
            sheriff.NotDefenseAgainstIndians(gameplay);
            var response = deputy.NotDefenseAgainstIndians(gameplay);
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response; 
            specificResponse.Team.Should().Be(Team.Renegade);
        }
        
        [Fact]
        public void When_Renegade_kills_last_Deputy_and_Sheriff_by_gatling_then_he_wins()
        {
            var gameplay = GameplayWithOutlawsEliminated(5);

            var sheriff = gameplay.FindPlayer(new Sheriff());
            var renegade = gameplay.FindPlayer(new Renegade());
            var deputy = gameplay.FindPlayer(new Deputy());
            
            sheriff.WithOneLifePoint();
            deputy.WithOneLifePoint();
            
            renegade.AddCardToHand(GatlingCard());
            gameplay.SetTurnToPlayer(renegade);
            
            // Act 
            renegade.PlayGatling(gameplay);
            sheriff.NotDefenseAgainstBang(gameplay);
            var response = deputy.NotDefenseAgainstBang(gameplay);
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response; 
            specificResponse.Team.Should().Be(Team.Renegade); 
        }

        [Fact]
        public void Game_can_be_over_after_dynamite_exploding()
        {
            var gamePlay = CreateGamePlay();
            var sheriff = gamePlay.FindPlayer(new Sheriff());
            
            sheriff.AddCardToHand(DynamiteCard());
            sheriff.WithOneLifePoint();
            sheriff.PlayDynamite(gamePlay);
            sheriff.EndTurn();
            
            gamePlay.SetTurnToPlayer(sheriff);
            gamePlay.PutCardOnDeck(ExplodeCard());

            // Act
            var response = gamePlay.StartPlayerTurn();

            response.Should().BeOfType<GameOverResponse>();
        }

        private BangGameCard DuelCard() => new DuelCardType().DiamondsTwo();
        private BangGameCard IndiansCard() => new IndiansCardType().DiamondsTwo();
        private BangGameCard GatlingCard() => new GatlingCardType().DiamondsTwo();
        private BangGameCard DynamiteCard() => new DynamiteCardType().DiamondsTwo();
        
        private BangGameCard ExplodeCard() => new MissedCardType().ClubsFive();

        private Gameplay CreateGamePlay(int playersAmount = 5)
        {
            return new GameplayBuilder(playersAmount)
                .WithoutCharacter(new KitCarlson())
                .WithoutCharacter(new PedroRamirez())
                .WithoutCharacter(new JessyJones())
                .Build();
        }

        private Gameplay GameplayWithDeputiesAndOutlawsEliminated()
        {
            var gamePlay = CreateGamePlay();

            foreach (var player in gamePlay.Players)
            {
                if (player.Role is Outlaw || player.Role is Deputy)
                    player.Die();
            }

            return gamePlay;
        }
        
        private Gameplay GameplayWithDeputiesEliminated()
        {
            var gamePlay = CreateGamePlay();

            foreach (var player in gamePlay.Players)
            {
                if (player.Role is Deputy)
                    player.Die();
            }

            return gamePlay;
        }
        
        private Gameplay GameplayWithOutlawsEliminated(int playersAmount = 5)
        {
            var gamePlay = CreateGamePlay(playersAmount);

            foreach (var player in gamePlay.Players)
            {
                if (player.Role is Outlaw)
                    player.Die();
            }

            return gamePlay;
        }
    }
}