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
    public class CatBalouSpecification
    {
        [Fact]
        public void Victim_of_Cat_Balou_card_always_have_any_card()
        {
            var gameplay = InitGame();
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
            var gameplay = InitGame();
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
            var gameplay = InitGame();
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
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var bangCard = new BangCardType().DiamondsThree();
            victim.AddCardToHand(bangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.ForceToDropRandomCard();
            
            // Assert
            victim.Hand.Should().NotContain(bangCard);
        }
        
        [Fact]
        public void Cat_Balou_can_force_to_drop_victim_active_cards()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.PlayerTablet.PutCard(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;
            
            // Act
            actor.ForceToDropCard(availableCards.ActiveCards.First());
            
            // Assert
            victim.ActiveCards.Should().NotContain(mustangCard);
        }
        
        [Fact]
        public void Victim_active_card_dropped_after_Cat_Balou_effect_goes_to_discard_pile()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.PlayerTablet.PutCard(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;

            var cardToDrop = availableCards.ActiveCards.First();
            
            // Act
            actor.ForceToDropCard(cardToDrop);
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(cardToDrop);
        }
        
        [Fact]
        public void Victim_hand_card_dropped_after_Cat_Balou_effect_goes_to_discard_pile()
        {
            var gameplay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gameplay);

            var catBalouCard = new CatBalouCardType().ClubsSeven();
            actor.AddCardToHand(catBalouCard);
            
            var mustangCard = new MustangCardType().DiamondsThree();
            victim.AddCardToHand(mustangCard);

            var availableCards = actor.PlayCard(catBalouCard, victim) as ChooseOneCardResponse;

            // Act
            actor.ForceToDropRandomCard();
            
            // Assert
            gameplay.GetTopCardFromDiscarded().Should().Be(mustangCard);
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
        
        private (Player actor, Player victim) ChoosePlayers(Game.Gameplay gameplay)
        {
            var actor = gameplay.PlayerTurn;
            actor.SetInfo(gameplay, actor.Role, new KitCarlson());

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            
            return (actor, victim);
        }
    }
}