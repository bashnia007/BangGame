using System;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class DuelSpecification
    {
        [Fact]
        public void Duel_card_is_playing_to_another_player()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            // Act
            Action act = () => actor.PlayCard(gamePlay, duelCard, actor);
            act.Should().Throw<InvalidOperationException>().WithMessage("* must be played to another player!");
        }
        
        [Fact]
        public void After_played_duel_is_discarded()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            // Act
            actor.PlayDuel(gamePlay, victim);

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(duelCard);
            actor.Hand.Should().NotContain(duelCard);
        }

        [Fact]
        public void If_victim_plays_does_not_defense_in_duel_he_or_she_will_lose_one_life()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayDuel(gamePlay, victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.NotDefenseAgainstBang(gamePlay);
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_and_attacker_does_not_defence_attacker_will_lose_life()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, _) = ChoosePlayers(gamePlay);
            
            actor.PlayDuel(gamePlay, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            var healthBefore = actor.PlayerTablet.Health;
            // Act
            victim.DefenseAgainstDuel(gamePlay, bangCard);
            actor.LoseDuel(gamePlay);
            
            // Assert
            actor.LifePoints.Should().Be(healthBefore - 1);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_then_it_will_be_discarded()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayDuel(gamePlay, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            // Act
            victim.DefenseAgainstDuel(gamePlay, bangCard);
            
            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(bangCard);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_then_victim_will_lose_bang_card()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayDuel(gamePlay, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            // Act
            victim.DefenseAgainstBang(gamePlay, bangCard);
            
            // Assert
            victim.Hand.Should().NotContain(bangCard);
        }

        [Fact]
        public void If_victim_and_attacker_play_by_one_bang_card_then_victim_will_lose_one_life()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim, _) = ChoosePlayers(gamePlay);
            
            var bangCard = new BangCardType().ClubsSeven();
            actor.AddCardToHand(bangCard);
            
            var anotherBangCard = new BangCardType().HeartsAce();
            victim.AddCardToHand(anotherBangCard);

            var healthBefore = victim.PlayerTablet.Health;
            
            actor.PlayDuel(gamePlay, victim);
            
            victim.DefenseAgainstDuel(gamePlay, anotherBangCard);
            actor.DefenseAgainstDuel(gamePlay, bangCard);
            
            // Act
            victim.LoseDuel(gamePlay);
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        private (Player actor, Player victim, BangGameCard duelCard) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            var duelCard = new DuelCardType().DiamondsThree();
            actor.AddCardToHand(duelCard);

            var victim = gameplay.Players.First(p => p != actor);
            
            return (actor, victim, duelCard);
        }
    }
}