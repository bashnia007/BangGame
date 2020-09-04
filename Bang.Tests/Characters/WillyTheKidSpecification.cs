using System;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;

using static Bang.Tests.TestUtils;

namespace Bang.Tests.Characters
{
    public class WillyTheKidSpecification
    {
        [Fact]
        public void Willy_The_Kid_starts_game_with_4_life_points()
        {
            var gamePlay = InitGameplayWithCharacter(new WillyTheKid());
            var (willyTheKid, _) = ChoosePlayers(gamePlay);

            willyTheKid.AsOutlaw(gamePlay);
            
            // Assert
            willyTheKid.LifePoints.Should().Be(4);
        }
        
        [Fact]
        public void Willy_the_kid_can_play_any_number_of_bang_cards_during_his_turn()
        {
            var gameplay = 
                new GameplayBuilder()
                    .WithCharacter(new WillyTheKid())
                    .WithoutCharacter(new Jourdonnais())
                    .Build();
            var (willy, willyNeighbour) = ChoosePlayers(gameplay);

            var firstBang = new BangCardType().ClubsFive();
            var secondBang = new BangCardType().ClubsSix();
            willy.AddCardToHand(firstBang);
            willy.AddCardToHand(secondBang);
            
            // Act
            willy.PlayCard(gameplay, firstBang, willyNeighbour);
            willyNeighbour.NotDefenseAgainstBang(gameplay);
            
            var response = willy.PlayCard(gameplay, secondBang, willyNeighbour);

            // Assert
            response.Should().NotBeOfType<NotAllowedOperation>();
        }
        
        private (Gameplay, Player, Player) InitGame(Gameplay gameplay)
        {
            var willyTheKid = gameplay.Players.First(p => p.Character is WillyTheKid);

            var willyNeighbour = gameplay.FindPlayerAtDistanceFrom(1, willyTheKid);

            return (gameplay, willyTheKid, willyNeighbour);
        }

        private (Player, Player) ChoosePlayers(Gameplay gameplay)
        {
            var (_, willy, willyNeighbour) = InitGame(gameplay);
            gameplay.SetTurnToPlayer(willy);

            return (willy, willyNeighbour);
        }
    }
}