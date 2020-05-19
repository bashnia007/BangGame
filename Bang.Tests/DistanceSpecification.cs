using System;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Gameplay.Characters;
using Gameplay.Roles;
using Xunit;
using Gameplay = Bang.Game.Gameplay;

namespace Bang.Tests
{
    public class DistanceSpecification
    {
        private Player CreatePlayer(Character character = null)
        {
            var player = new PlayerOnline(Guid.NewGuid().ToString());
            
            player.SetInfo(new Game.Gameplay(new Deck<Character>(), new Deck<BangGameCard>()), new Outlaw(), character?? new Jourdonnais());

            return player;
        }

        private Player CreatePaulRegretPlayer() => CreatePlayer(new PaulRegret());

        private Player CreateRoseDoolan() => CreatePlayer(new RoseDoolan());


        [Fact]
        public void Normal_distance_between_players_is_the_minimum_number_of_places_between_them()
        {
            var fromPlayer = CreatePlayer();
            var toPlayer = CreatePlayer(new BartCassidy());

            var alivePlayers = new[] {CreatePlayer(), fromPlayer, CreatePlayer(), CreatePlayer(), CreatePlayer(), toPlayer, CreatePlayer(), };
            // Act
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, toPlayer);
            
            // Assert 
            distance.Should().Be(3);
        }

        [Fact]
        public void Normal_distance_between_two_players_is_the_same()
        {
            var firstPlayer = CreatePlayer();
            var secondPlayer = CreatePlayer(new BlackJack());

            var alivePlayers = new[] {secondPlayer, CreatePlayer(), CreatePlayer(), firstPlayer, CreatePlayer()};
            
            // Act 
            var distanceFromFirst = DistanceCalculator.GetDistance(alivePlayers, firstPlayer, secondPlayer);
            var distanceFromSecond = DistanceCalculator.GetDistance(alivePlayers, secondPlayer, firstPlayer);

            distanceFromFirst.Should().Be(distanceFromSecond);
        }
        
        [Fact]
        public void Distance_can_not_be_less_than_one()
        {
            var fromPlayer = CreatePlayer();
            var toPlayer = CreatePlayer();

            var scopeCard = new ScopeCardType().ClubsSeven();
            fromPlayer.PlayerTablet.PutCard(scopeCard);
            
            var alivePlayers = new[] {toPlayer, fromPlayer};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, toPlayer);

            distance.Should().Be(1);
        }
        
        [Fact]
        public void Player_with_a_scope_in_play_sees_the_other_players_at_a_distance_decreased_by_one()
        {
            var playerWithScope = CreatePlayer();
            var toPlayer = CreatePlayer();
            
            var scopeCard = new ScopeCardType().SpadesQueen();
            playerWithScope.PlayerTablet.PutCard(scopeCard);

            var alivePlayers = new[] {playerWithScope, CreatePlayer(), toPlayer, CreatePlayer()};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, playerWithScope, toPlayer);

            distance.Should().Be(1);
        }
        
        [Fact]
        public void Players_see_a_player_with_a_scope_in_play_at_a_normal_distance()
        {
            var playerWithScope = CreatePlayer();
            var toPlayer = CreatePlayer();

            var scopeCard = new ScopeCardType().HeartsAce();
            
            playerWithScope.PlayerTablet.PutCard(scopeCard);

            var alivePlayers = new[] {playerWithScope, CreatePlayer(), toPlayer, CreatePlayer()};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, playerWithScope, toPlayer);

            distance.Should().Be(1);
        }
        
        
        [Fact]
        public void Players_see_a_player_with_a_mustang_in_play_at_a_distance_increased_by_one()
        {
            var fromPlayer = CreatePlayer();
            
            var toPlayer = CreatePlayer();
            var mustangCard = new MustangCardType().SpadesQueen();
            toPlayer.PlayerTablet.PutCard(mustangCard);

            var alivePlayers = new[] {fromPlayer, toPlayer};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, toPlayer);

            distance.Should().Be(2);
        }
        
        [Fact]
        public void Player_with_a_mustang_in_play_sees_the_other_players_at_the_normal_distance()
        {
            var fromPlayer = CreatePlayer();
            var mustangCard = new MustangCardType().DiamondsThree();
            fromPlayer.PlayerTablet.PutCard(mustangCard);
            
            var toPlayer = CreatePlayer();

            var alivePlayers = new[] {fromPlayer, CreatePlayer(), toPlayer, CreatePlayer()};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, toPlayer);

            distance.Should().Be(2);
        }

        [Fact]
        public void Paul_Regret_is_considered_to_have_a_mustang_in_play_at_all_times()
        {
            var fromPlayer = CreatePlayer();
            var paulRegretPlayer = CreatePaulRegretPlayer();
            
            var alivePlayers = new[] {fromPlayer, paulRegretPlayer};
            
            // Act
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, paulRegretPlayer);

            distance.Should().Be(2);
        }
        
        [Fact]
        public void Players_see_Paul_Regret_with_a_mustang_in_play_at_a_distance_increased_by_one()
        {
            var fromPlayer = CreatePlayer();

            var paulRegretPlayer = CreatePaulRegretPlayer();
            var mustangCard = new MustangCardType().DiamondsThree();
            paulRegretPlayer.PlayerTablet.PutCard(mustangCard);
            
            var alivePlayers = new[] {fromPlayer, paulRegretPlayer};
            
            // Act
            var distance = DistanceCalculator.GetDistance(alivePlayers, fromPlayer, paulRegretPlayer);

            distance.Should().Be(3);
        }

        [Fact]
        public void Doolan_Rosy_is_considered_to_have_a_scope_in_play_at_all_times()
        {
            var rosyPlayer = CreateRoseDoolan();
            var player = CreatePlayer();
            
            var alivePlayers = new[] {rosyPlayer, CreatePlayer(),  player, CreatePlayer()};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, rosyPlayer, player);

            distance.Should().Be(1);
        }

        [Fact]
        public void Doolan_Rosy_with_a_scope_in_play_sees_other_players_at_a_distance_decreased_by_two()
        {
            var player = CreatePlayer();
            var rosyPlayer = CreateRoseDoolan();
            var scopeCard = new ScopeCardType().ClubsSeven();
            rosyPlayer.PlayerTablet.PutCard(scopeCard);
            
            var alivePlayers = new[] {rosyPlayer, CreatePlayer(),  CreatePlayer(), player, CreatePlayer(), CreatePlayer()};
            
            // Act 
            var distance = DistanceCalculator.GetDistance(alivePlayers, rosyPlayer, player);

            distance.Should().Be(1);
        }
    }
}