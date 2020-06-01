using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class DuelSpecification
    {
        [Fact]
        public void Duel_card_is_playing_to_another_player()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            // Act
            Action act = () => actor.PlayCard(duelCard, actor);
            act.Should().Throw<InvalidOperationException>().WithMessage("* must be played to another player!");
        }
        
        [Fact]
        public void After_played_duel_is_discarded()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            // Act
            actor.PlayCard(duelCard, victim);

            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(duelCard);
            actor.Hand.Should().NotContain(duelCard);
        }

        [Fact]
        public void If_victim_plays_does_not_defense_in_duel_he_or_she_will_lose_one_life()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(duelCard, victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.NotDefense();
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_and_attacker_does_not_defence_attacker_will_lose_life()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(duelCard, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            var healthBefore = actor.PlayerTablet.Health;
            // Act
            victim.Defense(bangCard);
            actor.NotDefense();
            
            // Assert
            actor.LifePoints.Should().Be(healthBefore - 1);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_then_it_will_be_discarded()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(duelCard, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            // Act
            victim.Defense(bangCard);
            
            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(bangCard);
        }
        
        [Fact]
        public void If_victim_replies_with_bang_card_then_victim_will_lose_bang_card()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(duelCard, victim);
            
            var bangCard = new BangCardType().ClubsSeven();
            victim.AddCardToHand(bangCard);

            // Act
            victim.Defense(bangCard);
            
            // Assert
            victim.Hand.Should().NotContain(bangCard);
        }

        [Fact]
        public void If_victim_and_attacker_play_by_one_bang_card_then_victim_will_lose_one_life()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim, BangGameCard duelCard) = ChoosePlayers(gamePlay);
            
            var bangCard = new BangCardType().ClubsSeven();
            actor.AddCardToHand(bangCard);
            
            var anotherBangCard = new BangCardType().HeartsAce();
            victim.AddCardToHand(anotherBangCard);

            var healthBefore = victim.PlayerTablet.Health;
            
            actor.PlayCard(duelCard, victim);
            
            victim.Defense(anotherBangCard);
            actor.Defense(bangCard);
            
            // Act
            victim.NotDefense();
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        private Game.Gameplay InitGame() => InitGame(BangGameDeck());
        
        private Game.Gameplay InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }
            
            var gameplay = new Game.Gameplay(CharactersDeck(), deck);
            gameplay.Initialize(players);
            return gameplay;
        }
        
        private (Player actor, Player victim, BangGameCard duelCard) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            var duelCard = new DuelCardType().DiamondsThree();
            actor.AddCardToHand(duelCard);

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            
            return (actor, victim, duelCard);
        }
    }
}