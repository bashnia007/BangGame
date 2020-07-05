using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using Xunit;

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class VultureSamSpecification
    {
        [Fact]
        public void Vulture_Sam_starts_game_with_4_life_points()
        {
            var (gamePlay, vultureSam, _) = InitGame();

            vultureSam.SetInfo(gamePlay, new Outlaw(), vultureSam.Character);
            
            // Assert
            vultureSam.LifePoints.Should().Be(4);
        }
        
        [Fact]
        public void Whenever_a_character_is_eliminated_from_the_game_Sam_takes_all_active_cards_that_player_had()
        {
            var (sam, outLaw) = ChoosePlayers();
            
            var volcanic = new VolcanicCardType().ClubsSeven();
            outLaw.PlayerTablet.PutCard(volcanic);
            
            // Act
            outLaw.LoseLifePoint(5);
            
            // Assert
            outLaw.ActiveCards.Should().BeEmpty();
            sam.Hand.Should().Contain(volcanic);
        }
        
        [Fact]
        public void Whenever_a_character_is_eliminated_from_the_game_Sam_takes_all_hand_cards_that_player_had()
        {
            var (sam, outLaw) = ChoosePlayers();
            
            var missedCard = new MissedCardType().ClubsSeven();
            outLaw.AddCardToHand(missedCard);
            
            // Act
            outLaw.LoseLifePoint(5);
            
            // Assert
            outLaw.Hand.Should().BeEmpty();
            sam.Hand.Should().Contain(missedCard);
        }

        [Fact]
        public void
            When_Vulture_Sam_eliminates_a_Deputy_as_a_Sheriff_he_discards_all_his_cards_after_getting_the_cards_of_the_Deputy()
        {
            var (gamePlay, sheriffSam, deputy) = InitGame();
            sheriffSam.SetInfo(gamePlay, new Sheriff(), sheriffSam.Character);
            
            var volcanic = new VolcanicCardType().HeartsAce();
            deputy.AddCardToHand(volcanic);
            deputy.SetInfo(gamePlay, new Deputy(), deputy.Character);
            
            // Act
            deputy.LoseLifePoint(sheriffSam, 4);
            
            // Assert
            sheriffSam.Hand.Should().BeEmpty();
            sheriffSam.ActiveCards.Should().BeEmpty();
        }

        [Fact]
        public void
            When_the_Dynamite_explodes_eliminating_a_player_Vulture_Sam_does_not_draw_the_Dynamite_along_with_all_other_cards()
        {
            var deck = new Deck<BangGameCard>();
            var (gameplay, vultureSam, outLaw) = InitGame(deck);

            outLaw.LoseLifePoint(2);
            
            var dynamite = new DynamiteCardType().HeartsAce();
            outLaw.PlayerTablet.PutCard(dynamite);
            
            while (gameplay.GetNextPlayer() != outLaw)
                gameplay.SetNextPlayer();

            // Act
            deck.Put(new StagecoachCardType().ClubsSeven()); // card to explode
            gameplay.StartNextPlayerTurn();
            
            // Assert 
            vultureSam.Hand.Should().NotContain(dynamite);
        }
        
        private (Game.Gameplay, Player, Player) InitGame() => InitGame(BangGameDeck());
        
        private (Game.Gameplay, Player, Player) InitGame(Deck<BangGameCard> deck)
        {
            var players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }
            
            var charactersDeck = new Deck<Character>();
            charactersDeck.Put(new VultureSam());
            charactersDeck.Put(new KitCarlson());
            charactersDeck.Put(new RoseDoolan());
            charactersDeck.Put(new WillyTheKid());

            var gameplay = new Game.Gameplay(charactersDeck, deck);
            gameplay.Initialize(players);

            var vultureSam = players.First(p => p.Character is VultureSam);
            
            var outLaw = players.First(p => p != vultureSam && p.Role is Outlaw);

            return (gameplay, vultureSam, outLaw);
        }

        private (Player, Player) ChoosePlayers()
        {
            var (_, vultureSam, sheriff) = InitGame();

            return (vultureSam, sheriff);
        }
    }
}