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

using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
    public class PlayerTurnSpecification
    {
        [Fact]
        public void Player_can_play_only_one_bang_card_per_turn()
        {
            var gamePlay = InitGame();
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
            var gamePlay = InitGame();
            (Player actor, Player victim) = ChoosePlayers(gamePlay);
            var anotherBangCard = new BangCardType().DiamondsThree();
            actor.AddCardToHand(anotherBangCard);
            
            actor.PlayCard(BangCard(), victim);
            victim.NotDefense();

            gamePlay.SetNextPlayer();
            while (gamePlay.PlayerTurn != actor)
            {
                gamePlay.SetNextPlayer();
            }
            
            // Act
            var response = actor.PlayCard(anotherBangCard, victim);

            // Assert
            response.Should().BeOfType<DefenceAgainstBang>();
        }
        
        [Fact]
        public void Player_with_active_volcanic_can_play_multiple_bang_cards_per_turn()
        {
            var gamePlay = InitGame();
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
            var gamePlay = InitGame();
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
            var gamePlay = InitGame();
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

        private Game.Gameplay InitGame() => InitGame(GamePlayInitializer.BangGameDeck());
        
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
            actor.AddCardToHand(BangCard());
            actor.AddCardToHand(AnotherBangCard());
            actor.AddCardToHand(DuelCard());
            actor.AddCardToHand(GatlingCard());
            

            var victim = gameplay.Players.First(p => p != actor);
            victim.SetInfo(gameplay, actor.Role, new PedroRamirez());
            victim.AddCardToHand(MissedCard());
            
            return (actor, victim);
        }
    }
}