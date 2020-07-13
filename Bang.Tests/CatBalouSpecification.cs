using System.Linq;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;

using FluentAssertions;
using Xunit;
using static Bang.Tests.TestUtils;

namespace Bang.Tests
{
    public class CatBalouSpecification
    {
        [Fact]
        public void Victim_of_Cat_Balou_card_always_have_any_card()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            // act
            var response = actor.PlayCard(catBalouCard, victim);
            
            // assert
            response.Should().BeOfType(typeof(NotAllowedOperation));
        }
        
        [Fact]
        public void After_played_Cat_Balou_card_goes_to_discard_pile()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayCard(catBalouCard, victim);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(catBalouCard);
        }
         
        [Fact]
        public void Player_discards_Cat_Balou_card_after_it_played()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            victim.AddCardToHand(new BangCardType().DiamondsThree());

            // act
            actor.PlayCard(catBalouCard, victim);
            
            // Assert
            actor.Hand.Should().NotContain(catBalouCard);
        }

        [Fact]
        public void Cat_Balou_can_force_to_drop_card_from_victim_hand()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var bangCard = new BangCardType().DiamondsThree();
            victim.AddCardToHand(bangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.ForceToDropRandomCard(victim);
            
            // Assert
            victim.Hand.Should().NotContain(bangCard);
        }
        
        [Fact]
        public void Cat_Balou_can_force_to_drop_victim_active_cards()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.PlayerTablet.PutCard(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.ForceToDropCard(victim, availableCards.ActiveCards.First());
            
            // Assert
            victim.ActiveCards.Should().NotContain(mustangCard);
        }
        
        [Fact]
        public void Victim_active_card_dropped_after_Cat_Balou_effect_goes_to_discard_pile()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.PlayerTablet.PutCard(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;

            var cardToDrop = availableCards.ActiveCards.First();
            
            // Act
            actor.ForceToDropCard(victim, cardToDrop);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(cardToDrop);
        }
        
        [Fact]
        public void Victim_hand_card_dropped_after_Cat_Balou_effect_goes_to_discard_pile()
        {
            var gameplay = InitGameplay();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.AddCardToHand(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;

            // Act
            actor.ForceToDropRandomCard(victim);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(mustangCard);
        }
            
        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            var victim = gameplay.AlivePlayers.First(p => p != actor);
            
            return (actor, victim);
        }
    }
}