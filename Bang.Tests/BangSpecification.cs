using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class BangSpecification
    {
        [Fact]
        public void After_played_bang_is_discarded()
        {
            var gamePlay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            // Act
            actor.PlayBang(gamePlay, victim);

            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(BangCard());
            actor.Hand.Should().NotContain(BangCard());
        }

        [Fact]
        public void If_victim_plays_missed_card_he_or_she_will_not_lose_life_point()
        {
            var gamePlay = CreateGamePlay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayBang(gamePlay, victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.DefenseAgainstBang(gamePlay, MissedCard());
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore);
        }
        
        [Fact]
        public void If_victim_plays_missed_card_it_goes_to_discard_pile()
        {
            var gamePlay = CreateGamePlay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            victim.AddCardToHand(MissedCard());
            actor.PlayBang(gamePlay, victim);

            // Act
            victim.DefenseAgainstBang(gamePlay, MissedCard());
            
            // Assert
            gamePlay.PeekTopCardFromDiscarded().Should().Be(MissedCard());
        }
        
        [Fact]
        public void If_victim_plays_missed_card_he_discard_it()
        {
            var gamePlay = CreateGamePlay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayBang(gamePlay, victim);

            // Act
            victim.DefenseAgainstBang(gamePlay, MissedCard());
            
            // Assert
            victim.Hand.Should().NotContain(MissedCard());
        }
        
        [Fact]
        public void If_victim_does_not_play_missed_card_he_or_she_will_lose_life_point()
        {
            var gamePlay = CreateGamePlay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayBang(gamePlay, victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.NotDefenseAgainstBang(gamePlay);
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void If_victim_does_barrel_then_he_or_she_will_not_lose_life_point()
        {
            var gamePlay = CreateGamePlay();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            victim.PlayerTablet.PutCard(new BarrelCardType().SpadesQueen());
            
            var healthBefore = victim.PlayerTablet.Health;

            gamePlay.StartPlayerTurn();
            gamePlay.PutCardOnDeck(new StagecoachCardType().HeartsAce());
            // Act
            var response = actor.PlayBang(gamePlay, victim);
            
            // Assert
            response.Should().BeOfType<Done>(); 
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Players_trying_to_cancel_the_Slab_the_Killers_bang_need_to_play_two_missed_cards()
        {
            var gamePlay = 
                new GameplayBuilder()
                    .WithCharacter(new SlabTheKiller())
                    .WithoutCharacter(new Jourdonnais())
                    .Build();
            
            (Player slabTheKiller, Player victim) = ChoosePlayers(gamePlay, new SlabTheKiller());
            
            var anotherMissedCard = new MissedCardType().HeartsAce();
            victim.AddCardToHand(anotherMissedCard);
            
            var healthBefore = victim.PlayerTablet.Health;
            
            slabTheKiller.PlayBang(gamePlay, victim);
            
            // Act
            victim.DefenseAgainstBang(gamePlay, MissedCard(), anotherMissedCard);
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore);
            victim.Hand.Should().NotContain(MissedCard());
            victim.Hand.Should().NotContain(anotherMissedCard);
        }

        [Fact]
        public void Jourdonnais_is_considered_to_have_a_barrel_in_play_at_all_the_time()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());
            
            var gamePlay = 
                new GameplayBuilder()
                    .WithDeck(deck)
                    .WithCharacter(new Jourdonnais())
                    .Build();
            
            (Player actor, Player _) = ChoosePlayers(gamePlay);
            
            actor.PlayerTablet.PutCard(new WinchesterCardType().SpadesNine());

            var jourdonnais = gamePlay.FindPlayer(new Jourdonnais());
            var healthBefore = jourdonnais.PlayerTablet.Health;
            
            // Act
            var response = actor.PlayBang(gamePlay, jourdonnais);
            
            // Assert
            response.Should().BeOfType<Done>(); 
            jourdonnais.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Player_by_default_can_not_bang_on_players_at_distance_two()
        {
            var gamePlay = new GameplayBuilder()
                .WithoutCharacter(new PaulRegret())
                .WithoutCharacter(new RoseDoolan())
                .Build();
            
            var hitter = gamePlay.PlayerTurn;
            var victim = gamePlay.FindPlayerAtDistanceFrom(2, hitter);
            
            hitter.AddCardToHand(BangCard());
            
            // Act
            var response = hitter.PlayBang(gamePlay, victim);
            
            // Assert
            response.Should().BeOfType<NotAllowedOperation>();
            ((NotAllowedOperation)response).Reason.Should().Contain("maximum reachable shooting distance is");
        }
        
        [Fact]
        public void Weapon_changes_shooting_reachable_distance()
        {
            var gamePlay = new GameplayBuilder()
                .WithoutCharacter(new PaulRegret())
                .WithoutCharacter(new RoseDoolan())
                .Build();
            
            var hitter = gamePlay.PlayerTurn;
            var target = gamePlay.FindPlayerAtDistanceFrom(2, hitter);
            
            hitter.AddCardToHand(RemingtonCard());
            hitter.AddCardToHand(BangCard());

            hitter.PlayRemington(gamePlay);
            // Act
            var response = hitter.PlayBang(gamePlay, target);
            
            // Assert
            response.Should().NotBeOfType<NotAllowedOperation>();
        }
        
        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().HeartsJack();
        private BangGameCard RemingtonCard() => new RemingtonCardType().ClubsSix();

        private Gameplay CreateGamePlay()
        {
            var builder = new GameplayBuilder();

            return builder.WithoutCharacter(new Jourdonnais())
                    .WithoutCharacter(new SlabTheKiller())
                    .Build();
        }
        
        private (Player actor, Player victim) ChoosePlayers(Gameplay gameplay, Character character = null)
        {
            Player actor = 
                character != null ? gameplay.SetTurnToCharacter(character) : gameplay.PlayerTurn;
            
            actor.AddCardToHand(BangCard());

            var victim = gameplay.FindPlayerAtDistanceFrom(1, actor);
            victim.AddCardToHand(MissedCard());
            
            return (actor, victim);
        }
    }
}
