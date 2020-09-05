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
    public class PanicSpecification
    {
        [Fact]
        public void Victim_of_panic_card_always_has_any_card()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            // act
            var response = actor.PlayPanic(gameplay, victim);
            
            // assert
            response.Should().BeOfType(typeof(NotAllowedOperation));
        }
        
        [Fact]
        public void Victim_of_panic_card_is_located_at_distance_one()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            victim.PlayerTablet.PutCard(new MustangCardType().HeartsAce());
            
            // act
            var response = actor.PlayPanic(gameplay, victim);
            
            // assert
            response.Should().BeOfType(typeof(NotAllowedOperation));
        }
        
        [Fact]
        public void After_played_panic_card_goes_to_discard_pile()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayPanic(gameplay, victim);
            
            // Assert
            gameplay.PeekTopCardFromDiscarded().Should().Be(panicCard);
        }
         
        [Fact]
        public void Player_discards_panic_card_after_it_played()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayPanic(gameplay, victim);
            
            // Assert
            actor.Hand.Should().NotContain(panicCard);
        }

        [Fact]
        public void Player_can_draw_active_card_via_panic_card()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.PlayerTablet.PutCard(volcanic);

            var availableCards = (ChooseOneCardResponse) actor.PlayPanic(gameplay, victim);
            
            // Act
            actor.DrawCardFromPlayer(gameplay, victim, availableCards.ActiveCards[0]);
     
            // Assert
            actor.Hand.Should().Contain(volcanic);
        }
        
        [Fact]
        public void Victim_can_lose_active_card_after_panic()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.PlayerTablet.PutCard(volcanic);

            var availableCards = (ChooseOneCardResponse) actor.PlayPanic(gameplay, victim);
            
            // Act
            actor.DrawCardFromPlayer(gameplay, victim, availableCards.ActiveCards[0]);
            
            // Assert
            victim.ActiveCards.Should().NotContain(volcanic);
        }
        
        [Fact]
        public void Player_can_draw_hand_card_via_panic_card()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            var gatlingCard = new GatlingCardType().DiamondsThree();
            victim.AddCardToHand(gatlingCard);

            var availableCards = actor.PlayPanic(gameplay, victim);

            // Act
            actor.DrawCardFromPlayer(gameplay, victim);
            
            // Assert
            actor.Hand.Should().Contain(gatlingCard);
        }
        
        [Fact]
        public void Victim_can_lose_card_from_hand_after_panic()
        {
            var gameplay = CreateGamePlay();
            var (actor, victim, panicCard) = ChoosePlayers(gameplay);

            var volcanic = new VolcanicCardType().SpadesQueen();
            victim.AddCardToHand(volcanic);

            var availableCards = actor.PlayPanic(gameplay, victim);
            
            // Act
            actor.DrawCardFromPlayer(gameplay, victim);
            
            // Assert
            victim.Hand.Should().BeEmpty();
        }

        private Gameplay CreateGamePlay()
        {
            return new GameplayBuilder()
                .WithoutCharacter(new RoseDoolan())
                .WithoutCharacter(new PaulRegret())
                .WithoutCharacter(new KitCarlson())
                .WithoutCharacter(new SuzyLafayette())
                .Build();
        }
            
        private (Player actor, Player victim, BangGameCard panicCard) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;

            var panicCard = new PanicCardType().ClubsSeven();
            actor.AddCardToHand(panicCard);

            var victim = gameplay.Players.First(p => p != actor);
            
            return (actor, victim, panicCard);
        }
    }
}