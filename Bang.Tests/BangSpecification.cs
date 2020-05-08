using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

using static Bang.Game.GameInitializer;

namespace Bang.Tests
{
    public class BangSpecification
    {
        [Fact]
        public void After_played_bang_is_discarded()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            // Act
            actor.PlayCard(BangCard(), victim);

            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(BangCard());
            actor.Hand.Should().NotContain(BangCard());
        }

        [Fact]
        public void If_victim_plays_missed_card_he_or_she_will_not_lose_life_point()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(BangCard(), victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.Defense(MissedCard());
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore);
        }
        
        [Fact]
        public void If_victim_plays_missed_card_it_goes_to_discard_pile()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            victim.AddCardToHand(MissedCard());
            actor.PlayCard(BangCard(), victim);

            // Act
            victim.Defense(MissedCard());
            
            // Assert
            gamePlay.GetTopCardFromDiscarded().Should().Be(MissedCard());
        }
        
        [Fact]
        public void If_victim_plays_missed_card_he_discard_it()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(BangCard(), victim);

            // Act
            victim.Defense(MissedCard());
            
            // Assert
            victim.Hand.Should().NotContain(MissedCard());
        }
        
        [Fact]
        public void If_victim_does_not_play_missed_card_he_or_she_will_lose_life_point()
        {
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            
            actor.PlayCard(BangCard(), victim);

            var healthBefore = victim.PlayerTablet.Health;
            // Act
            victim.NotDefense();
            
            // Assert
            victim.LifePoints.Should().Be(healthBefore - 1);
        }

        [Fact]
        public void If_victim_does_barrel_then_he_or_she_will_not_lose_life_point()
        {
            var deck = new Deck<BangGameCard>();
            deck.Put(new StagecoachCardType().HeartsAce());
            
            var gamePlay = InitGame(deck);
            (Player actor, Player victim) = ChoosePlayers(gamePlay);

            var barrelCard = new BarrelCardType().SpadesQueen();
            victim.AddCardToHand(barrelCard);
            victim.PlayCard(barrelCard);
            
            var healthBefore = victim.PlayerTablet.Health;
            
            // Act
            var response = actor.PlayCard(BangCard(), victim);
            
            // Assert
            response.Should().BeOfType<Done>(); 
            victim.LifePoints.Should().Be(healthBefore);
        }

        [Fact]
        public void Players_trying_to_cancel_the_Slab_the_Killers_bang_need_to_play_two_missed_cards()
        {
            var gamePlay = InitGame();
            (Player slabTheKiller, Player victim) = ChoosePlayers(gamePlay);
            
            slabTheKiller.SetInfo(gamePlay, slabTheKiller.Role, new SlabTheKiller());
            
            var anotherMissedCard = new MissedCardType().HeartsAce();
            victim.AddCardToHand(anotherMissedCard);
            
            var healthBefore = victim.PlayerTablet.Health;
            
            slabTheKiller.PlayCard(BangCard(), victim);
            
            // Act
            victim.Defense(MissedCard(), anotherMissedCard);
            
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
            
            var gamePlay = InitGame(deck);
            (Player actor, Player jourdonnais) = ChoosePlayers(gamePlay);

            jourdonnais.SetInfo(gamePlay, jourdonnais.Role, new Jourdonnais());
            
            var healthBefore = jourdonnais.PlayerTablet.Health;
            
            // Act
            var response = actor.PlayCard(BangCard(), jourdonnais);
            
            // Assert
            response.Should().BeOfType<Done>(); 
            jourdonnais.LifePoints.Should().Be(healthBefore);
        }
        
        private BangGameCard BangCard() => new BangCardType().SpadesQueen();
        private BangGameCard MissedCard() => new MissedCardType().SpadesQueen();

        private Game.Gameplay InitGame() => InitGame(GameInitializer.BangGameDeck());
        
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
        
        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.AddCardToHand(BangCard());

            var victim = gameplay.Players.First(p => p != actor);
            victim.AddCardToHand(MissedCard());
            
            return (actor, victim);
        }
    }
}
