using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class PanicSpecification
    {
        [Fact]
        public void Victim_of_panic_card_always_has_any_card()
        {
            var gameplay = InitGame();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            // act
            var response = actor.PlayCard(panicCard, victim);
            
            // assert
            response.Should().BeOfType(typeof(NotAllowedOperation));
        }
        
        [Fact]
        public void Victim_of_panic_card_is_located_at_distance_one()
        {
            var gameplay = InitGame();
            (var actor, var victim, var panicCard) = ChoosePlayers(gameplay);

            victim.PlayerTablet.PutCard(new MustangCardType().HeartsAce());
            
            // act
            var response = actor.PlayCard(panicCard, victim);
            
            // assert
            response.Should().BeOfType(typeof(NotAllowedOperation));
        }
        
        [Fact]
        public void After_played_panic_card_goes_to_discard_pile()
        {
            var gameplay = InitGame();
            (var actor, var victim, var panicCard) = ChoosePlayers(gameplay);

            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayCard(panicCard, victim);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(panicCard);
        }
         
        [Fact]
        public void Player_discards_panic_card_after_it_played()
        {
            var gameplay = InitGame();
            (var actor, var victim, var panicCard) = ChoosePlayers(gameplay);

            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayCard(panicCard, victim);
            
            // Assert
            actor.Hand.Should().NotContain(panicCard);
        }

        [Fact]
        public void Player_can_draw_active_card_via_panic_card()
        {
            var gameplay = InitGame();
            (var actor, var victim, var panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.PlayerTablet.PutCard(volcanic);

            var availableCards = actor.PlayCard(panicCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.DrawCardFromPlayer(victim, availableCards.ActiveCards[0]);
            
            // Assert
            actor.Hand.Should().Contain(volcanic);
        }
        
        [Fact]
        public void Victim_can_lose_active_card_after_panic()
        {
            var gameplay = InitGame();
            (Player actor, Player victim, var panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.PlayerTablet.PutCard(volcanic);

            var availableCards = actor.PlayCard(panicCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.DrawCardFromPlayer(victim, availableCards.ActiveCards[0]);
            
            // Assert
            victim.ActiveCards.Should().NotContain(volcanic);
        }
        
        [Fact]
        public void Player_can_draw_hand_card_via_panic_card()
        {
            var gameplay = InitGame();
            (Player actor, Player victim, BangGameCard panicCard) = ChoosePlayers(gameplay);

            var gatlingCard = new GatlingCardType().DiamondsThree();
            victim.AddCardToHand(gatlingCard);

            var availableCards = actor.PlayCard(panicCard, victim) as ChooseOneCardResponse;

            // Act
            actor.DrawCardFromPlayer(victim);
            
            // Assert
            actor.Hand.Should().Contain(gatlingCard);
        }
        
        [Fact]
        public void Victim_can_lose_card_from_hand_after_panic()
        {
            var gameplay = InitGame();
            (Player actor, Player victim, var panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.AddCardToHand(volcanic);

            var availableCards = actor.PlayCard(panicCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.DrawCardFromPlayer(victim);
            
            // Assert
            victim.Hand.Should().BeEmpty();
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
        
        private (Player actor, Player victim, BangGameCard panicCard) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());
            
            var panicCard = new PanicCardType().ClubsSeven();
            actor.AddCardToHand(panicCard);

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            
            return (actor, victim, panicCard);
        }
    }
}