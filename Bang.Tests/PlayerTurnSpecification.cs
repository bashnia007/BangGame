using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

namespace Bang.Tests
{
    public class PlayerTurnSpecification
    {
        [Fact]
        public void Player_can_play_only_one_bang_card_per_turn()
        {
            var gamePlay = CreateGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            // Act
            actor.PlayCard(BangCard(), victim);
            victim.NotDefense();
            var response = actor.PlayCard(AnotherBangCard(), victim);

            // Assert
            response.Should().BeOfType<NotAllowedOperation>();
        }
        
        [Fact]
        public void If_player_plays_bang_card_he_can_play_another_bang_card_in_his_next_turn()
        {
            var gamePlay = CreateGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            var anotherBangCard = new BangCardType().DiamondsThree();
            actor.AddCardToHand(anotherBangCard);
            
            actor.PlayCard(BangCard(), victim);
            victim.NotDefense();

            gamePlay.SkipTurnsUntilPlayer(actor);
            gamePlay.SetTurnToPlayer(actor);
            
            // Act
            var response = actor.PlayCard(anotherBangCard, victim);

            // Assert
            response.Should().BeOfType<DefenceAgainstBang>();
        }
        
        [Fact]
        public void Player_with_active_volcanic_can_play_multiple_bang_cards_per_turn()
        {
            var gamePlay = CreateGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            var volcanic = new VolcanicCardType().ClubsSeven();
            actor.AddCardToHand(volcanic);
            actor.PlayCard(volcanic);
            
            var anotherBangCard = new BangCardType().DiamondsThree();
            actor.AddCardToHand(anotherBangCard);

            // Act
            actor.PlayCard(BangCard(), victim);
            victim.NotDefense();
            var response = actor.PlayCard(anotherBangCard, victim);

            // Assert
            response.Should().BeOfType<DefenceAgainstBang>();
        }

        [Fact]
        public void Player_can_play_bang_and_duel_per_turn()
        {
            var gamePlay = CreateGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            // Act
            actor.PlayCard(DuelCard(), victim);
            
            victim.NotDefense();
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<DefenceAgainstBang>();
        }
        
        [Fact]
        public void Player_can_play_bang_and_gatling_per_turn()
        {
            var gamePlay = CreateGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            // Act
            actor.PlayCard(GatlingCard());

            foreach (var gatlingVictim in gamePlay.Players.Where(p => p != actor))
            {
                gatlingVictim.NotDefense();
            }
            
            var response = actor.PlayCard(BangCard(), victim);

            // Assert
            response.Should().BeOfType<DefenceAgainstBang>();
        }
        
        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard AnotherBangCard() => new BangCardType().HeartsAce();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();
        private BangGameCard DuelCard() => new DuelCardType().SpadesQueen();
        private BangGameCard GatlingCard() => new GatlingCardType().SpadesQueen();

        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(BangCard());
            actor.AddCardToHand(AnotherBangCard());
            actor.AddCardToHand(DuelCard());
            actor.AddCardToHand(GatlingCard());
            

            var victim = gameplay.FindPlayerAtDistanceFrom(1, actor);
            victim.AddCardToHand(MissedCard());
            
            return (actor, victim);
        }

        private Gameplay CreateGameplay()
        {
            return new GameplayBuilder()
                .WithoutCharacter(new ElGringo())
                .WithoutCharacter(new WillyTheKid())
                .WithoutCharacter(new Jourdonnais())
                .Build();
        }
    }
}