using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
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
            var (gameplay, sheriff, renegade) = GameplayWithDeputiesAndOutlawsEliminated();

            var expectedWinners = gameplay.Players.Where(p => p.Role is Sheriff || p.Role is Deputy);
            
            renegade.WithOneLifePoint();
            
            sheriff.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(sheriff);
            
            // Act 
            sheriff.PlayDuel(renegade);
            var response = renegade.NotDefense();
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();

            var specificResponse = ((GameOverResponse) response); 
            specificResponse.Winners.Should().BeEquivalentTo(expectedWinners); 
        }
        
        [Fact]
        public void If_the_Renegade_is_the_only_one_alive_then_he_wins()
        {
            var (gameplay, sheriff, renegade) = GameplayWithDeputiesAndOutlawsEliminated();

            sheriff.WithOneLifePoint();
            
            renegade.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(renegade);
            
            // Act 
            renegade.PlayDuel(sheriff);
            var response = sheriff.NotDefense();
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = ((GameOverResponse) response); 
            specificResponse.Winners.Should().OnlyContain(p => p == renegade); 
        }
        
        [Fact]
        public void If_Sheriff_killed_and_non_Renegade_player_is_alive_then_Outlaws_win()
        {
            var (gameplay, sheriff) = GameplayWithDeputiesEliminated();

            var expectedWinners = gameplay.Players.Where(p => p.Role is Outlaw);
            
            var outlaw = gameplay.FindPlayer(new Outlaw());
            
            sheriff.WithOneLifePoint();
            
            outlaw.AddCardToHand(DuelCard());
            gameplay.SetTurnToPlayer(outlaw);
            
            // Act 
            outlaw.PlayDuel(sheriff);
            var response = sheriff.NotDefense();
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response; 
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
            renegade.PlayIndians();
            sheriff.NotDefense();
            var response = deputy.NotDefense();
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response; 
            specificResponse.Winners.Should().BeEquivalentTo(renegade); 
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
            renegade.PlayGatling();
            sheriff.NotDefense();
            var response = deputy.NotDefense();
            
            // Assert 
            response.Should().BeOfType<GameOverResponse>();
            
            var specificResponse = (GameOverResponse) response; 
            specificResponse.Winners.Should().BeEquivalentTo(renegade); 
        }

        [Fact]
        public void Game_can_be_over_after_dynamite_exploding()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(ExplodeCard());
            var gamePlay = CreateGamePlay(deck);
            var sheriff = gamePlay.FindPlayer(new Sheriff());
            
            sheriff.AddCardToHand(DynamiteCard());
            sheriff.WithOneLifePoint();
            sheriff.PlayDynamite();
            
            gamePlay.SkipTurnsUntilPlayer(sheriff);

            // Act
            var response = gamePlay.StartNextPlayerTurn();

            response.Should().BeOfType<GameOverResponse>();
        }

        private BangGameCard DuelCard() => new DuelCardType().DiamondsTwo();
        private BangGameCard IndiansCard() => new IndiansCardType().DiamondsTwo();
        private BangGameCard GatlingCard() => new GatlingCardType().DiamondsTwo();
        private BangGameCard DynamiteCard() => new DynamiteCardType().DiamondsTwo();
        
        private BangGameCard ExplodeCard() => new MissedCardType().ClubsFive();

        private Gameplay CreateGamePlay(Deck<BangGameCard> deck)
        {
            return new GameplayBuilder()
                .WithDeck(deck)
                .WithoutCharacter(new KitCarlson())
                .Build();
        }
        
        private Gameplay CreateGamePlay(int playersAmount = 5)
        {
            return new GameplayBuilder(playersAmount).Build();
        }

        private (Gameplay, Player, Player) GameplayWithDeputiesAndOutlawsEliminated()
        {
            var gamePlay = CreateGamePlay();

            Player sheriff = null;
            Player renegade = null;
            foreach (var player in gamePlay.Players)
            {
                if (player.Role is Outlaw || player.Role is Deputy)
                    player.Die();

                else if (player.Role is Sheriff)
                {
                    sheriff = player;
                } 
                else if (player.Role is Renegade)
                {
                    renegade = player;
                }
            }

            return (gamePlay, sheriff, renegade);
        }
        
        private (Gameplay, Player) GameplayWithDeputiesEliminated()
        {
            var gamePlay = CreateGamePlay();

            Player sheriff = null;
            Player renegade = null;
            foreach (var player in gamePlay.Players)
            {
                if (player.Role is Deputy)
                    player.Die();

                else if (player.Role is Sheriff)
                {
                    sheriff = player;
                } 
            }

            return (gamePlay, sheriff);
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