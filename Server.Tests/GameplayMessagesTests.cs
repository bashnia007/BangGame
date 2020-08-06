using System;
using System.Linq;
using Bang.Characters;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using FluentAssertions;
using Server.Messages;
using Xunit;

namespace Server.Tests
{
    public class GameplayMessagesTests
    {
        [Fact]
        public void Bang_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var bang = new BangCardType().ClubsSeven();

            player.AddCardToHand(bang);
            var playerHandBefore = player.Hand.Count;

            var missed = new MissedCardType().DiamondsThree();
            var victim = game.Gameplay.Players.First(p => p != player);
            victim.AddCardToHand(missed);

            var opponentHandSizeBefore = victim.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, bang, victim);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);
            var m = (DefenceAgainstBang) playCardResponse.Response;

            playCardResponse.Player.Should().BeSameAs(victim);
            m.ReplyTo.Should().BeSameAs(playCardMessage);
            m.Player.Should().BeSameAs(victim);

            var defenseAgainstBang = playCardResponse.ReplyWithDefenceAgainstBang(victim, missed);

            var defenseAgainstBangResponse = game.ProcessEvent(defenseAgainstBang);
            defenseAgainstBangResponse.Should().BeOfType<ActionDoneMessage>();

            player.Hand.Should().HaveCountLessThan(playerHandBefore);
            victim.Hand.Should().HaveCountLessThan(opponentHandSizeBefore);
        }

        [Fact]
        public void Barrel_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var barrel = new BarrelCardType().ClubsSeven();

            player.AddCardToHand(barrel);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, barrel);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Beer_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var beer = new BeerCardType().ClubsSeven();

            player.AddCardToHand(beer);

            // Act
            var playCardMessage = new PlayCardMessage(player, beer);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            playCardResponse.Should().BeOfType<ActionDoneMessage>();
        }

        [Fact]
        public void Carabine_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var carabine = new CarabineCardType().ClubsSeven();

            player.AddCardToHand(carabine);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, carabine);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void CatBalou_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var catBalouCard = new CatBalouCardType().ClubsSeven();

            player.AddCardToHand(catBalouCard);

            var victim = game.Gameplay.Players.First(p => p != player);

            var opponentHandSizeBefore = victim.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, catBalouCard, victim);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);
            var m = (ChooseOneCardResponse) playCardResponse.Response;

            playCardResponse.Player.Should().BeSameAs(player);
            m.ReplyTo.Should().BeSameAs(playCardMessage);
            m.Player.Should().BeSameAs(player);

            var forceToDropCardMessage = playCardResponse.ReplyWithForcingToDrop(victim);

            var forceToDropResponse = game.ProcessEvent(forceToDropCardMessage);
            forceToDropResponse.Should().BeOfType<ActionDoneMessage>();

            victim.Hand.Should().HaveCountLessThan(opponentHandSizeBefore);
        }

        [Fact]
        public void Duel_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var duelCard = new DuelCardType().ClubsSeven();
            var firstBang = new BangCardType().HeartsAce();
            var secondBang = new BangCardType().SpadesQueen();

            player.AddCardToHand(duelCard);
            player.AddCardToHand(firstBang);

            var opponent = game.Gameplay.Players.First(p => p != player);
            opponent.AddCardToHand(secondBang);

            // Act
            var message = new PlayCardMessage(player, duelCard, opponent);
            var defenceAgainstDuel = game.ProcessEvent(message);

            defenceAgainstDuel.Player.Should().BeSameAs(opponent);
            player.Hand.Should().NotContain(duelCard);

            var opponentResponse = defenceAgainstDuel.ReplyWithDefenceAgainstDuel(opponent, secondBang);

            var responseToActor = game.ProcessEvent(opponentResponse);

            responseToActor.Player.Should().BeSameAs(player);
            opponent.Hand.Should().NotContain(secondBang);

            var playerResponse = responseToActor.ReplyWithDefenceAgainstDuel(player, firstBang);
            var response = game.ProcessEvent(playerResponse);
            
            player.Hand.Should().NotContain(firstBang);
            
            // opponent lose duel
            var loseDuelMessage = response.ReplyWithDefenceAgainstDuel(opponent, null);
            var finalMessage = game.ProcessEvent(loseDuelMessage);

            finalMessage.Should().BeOfType<ActionDoneMessage>();
        }

        [Fact]
        public void Dynamite_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var dynamite = new DynamiteCardType().ClubsSeven();

            player.AddCardToHand(dynamite);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, dynamite);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Gatling_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var gatling = new GatlingCardType().ClubsSeven();

            player.AddCardToHand(gatling);
            var handBefore = player.Hand.Count;

            var missedCard = new MissedCardType().HeartsAce();

            var victimWithMissedCard = game.Gameplay.Players.First(p => p != player);
            victimWithMissedCard.AddCardToHand(missedCard);

            var victimHandBefore = victimWithMissedCard.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, gatling);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);
            playCardResponse.Response.Should().BeOfType<MultiplayerDefenceResponse>();

            var defenseAgainstBang = playCardResponse.ReplyWithDefenceAgainstBang(victimWithMissedCard, missedCard);

            var responseToVictim = (ReplyActionMessage) game.ProcessEvent(defenseAgainstBang);
            responseToVictim.Response.Should().BeOfType<MultiplayerDefenceResponse>();

            BangGameMessage lastReply = null;
            
            foreach (var otherPlayer in game.Gameplay.Players.Where(p => p != player && p != victimWithMissedCard))
            {
                var message = playCardResponse.ReplyWithDefenceAgainstBang(otherPlayer, null);
                lastReply = game.ProcessEvent(message);
            }

            lastReply.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
            victimWithMissedCard.Hand.Should().HaveCountLessThan(victimHandBefore);
        }
        
        [Fact]
        public void GeneralStore_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var generalStore = new GeneralStoreCardType().ClubsSeven();

            player.AddCardToHand(generalStore);
            var handBefore = player.Hand.Count;

            var playCardMessage = new PlayCardMessage(player, generalStore);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);

            playCardResponse.Response.Should().BeOfType<ChooseCardsResponse>();
            var response = (ChooseCardsResponse) playCardResponse.Response;
            response.CardsToChoose.Should().HaveCount(game.Gameplay.Players.Count);

            var playerToChoose = response.PlayerTurn;
            var chooseCardMessage =
                playCardResponse.ReplyWithTakingCardFromBoard(playerToChoose, response.CardsToChoose[0]);
            
            BangGameMessage lastReply = (ReplyActionMessage) game.ProcessEvent(chooseCardMessage);
            ((ReplyActionMessage) lastReply).Response.Should().BeOfType<ChooseCardsResponse>();
            player.Hand.Should().HaveCount(handBefore);

            while (lastReply is ReplyActionMessage)
            {
                response = (ChooseCardsResponse) (lastReply as ReplyActionMessage).Response;
                playerToChoose = response.PlayerTurn;
                chooseCardMessage = lastReply.ReplyWithTakingCardFromBoard(playerToChoose, response.CardsToChoose[0]);
                lastReply = game.ProcessEvent(chooseCardMessage);
            }

            lastReply.Should().BeOfType<ActionDoneMessage>();
        }
        
        [Fact]
        public void Indians_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var indians = new IndiansCardType().ClubsSeven();

            player.AddCardToHand(indians);
            var handBefore = player.Hand.Count;

            var bangCard = new BangCardType().HeartsAce();

            var victimWithBangCard = game.Gameplay.Players.First(p => p != player);
            victimWithBangCard.AddCardToHand(bangCard);

            var victimHandBefore = victimWithBangCard.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, indians);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);

            playCardResponse.Response.Should().BeOfType<MultiplayerDefenceResponse>();

            var defenseAgainstIndians = 
                playCardResponse.ReplyWithDefenceAgainstIndians(victimWithBangCard, bangCard);
            
            var responseToVictim = (ReplyActionMessage) game.ProcessEvent(defenseAgainstIndians);
            responseToVictim.Response.Should().BeOfType<MultiplayerDefenceResponse>();

            BangGameMessage lastReply = null;
            
            foreach (var otherPlayer in game.Gameplay.Players.Where(p => p != player && p != victimWithBangCard))
            {
                var message = playCardResponse.ReplyWithDefenceAgainstIndians(otherPlayer);
                lastReply = game.ProcessEvent(message);
            }

            lastReply.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
            victimWithBangCard.Hand.Should().HaveCountLessThan(victimHandBefore);
        }

        [Fact]
        public void Jail_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var jail = new JailCardType().ClubsSeven();

            var opponent = game.Gameplay.Players.First(p => p != player);

            player.AddCardToHand(jail);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, jail, opponent);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Mustang_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var mustang = new MustangCardType().ClubsSeven();

            player.AddCardToHand(mustang);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, mustang);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Panic_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var panicCard = new PanicCardType().ClubsSeven();

            var playerHandSizeBefore = player.Hand.Count;

            player.AddCardToHand(panicCard);

            var victim = game.Gameplay.Players.First(p => p != player);

            var opponentHandSizeBefore = victim.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, panicCard, victim);
            var playCardResponse = (ReplyActionMessage) game.ProcessEvent(playCardMessage);
            var m = (ChooseOneCardResponse) playCardResponse.Response;

            playCardResponse.Player.Should().BeSameAs(player);
            m.ReplyTo.Should().BeSameAs(playCardMessage);
            m.Player.Should().BeSameAs(player);

            var stealRandomCardMessage = new ReplyActionMessage(victim)
            {
                Response = new DrawCardFromPlayerResponse() {Player = victim, ReplyTo = playCardResponse}
            };

            var stealingResponse = game.ProcessEvent(stealRandomCardMessage);
            
            stealingResponse.Should().BeOfType<ActionDoneMessage>();

            player.Hand.Should().HaveCountGreaterThan(playerHandSizeBefore);
            victim.Hand.Should().HaveCountLessThan(opponentHandSizeBefore);
        }

        [Fact]
        public void Remington_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var remingtonCard = new RemingtonCardType().ClubsSeven();

            player.AddCardToHand(remingtonCard);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, remingtonCard);
            var playCardResponse = (ActionDoneMessage) game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Saloon_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var saloonCard = new SaloonCardType().ClubsSeven();

            player.AddCardToHand(saloonCard);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, saloonCard);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Schofield_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var remingtonCard = new SchofieldCardType().ClubsSeven();

            player.AddCardToHand(remingtonCard);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, remingtonCard);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Scope_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var scope = new ScopeCardType().ClubsSeven();

            player.AddCardToHand(scope);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, scope);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Stagecoach_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var stagecoach = new StagecoachCardType().ClubsSeven();

            player.AddCardToHand(stagecoach);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, stagecoach);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountGreaterThan(handBefore);
        }

        [Fact]
        public void Volcanic_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var volcanic = new VolcanicCardType().ClubsSeven();

            player.AddCardToHand(volcanic);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, volcanic);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Wells_Fargo_Card_Scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var wellsFargo = new WellsFargoCardType().ClubsSeven();

            player.AddCardToHand(wellsFargo);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, wellsFargo);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountGreaterThan(handBefore);
        }

        [Fact]
        public void Winchester_card_scenario()
        {
            var game = CreateAndStartGame();
            var player = game.Gameplay.PlayerTurn;
            var winchester = new WinchesterCardType().ClubsSeven();

            player.AddCardToHand(winchester);
            var handBefore = player.Hand.Count;

            // Act
            var playCardMessage = new PlayCardMessage(player, winchester);
            var playCardResponse = game.ProcessEvent(playCardMessage);

            // Assert
            playCardResponse.Should().BeOfType<ActionDoneMessage>();
            player.Hand.Should().HaveCountLessThan(handBefore);
        }

        [Fact]
        public void Next_player_turn_scenario()
        {
            var game = CreateAndStartGame();
            var nextPlayer = game.Gameplay.GetNextPlayer();

            var nextPlayerTurnMessage = new NextPlayerTurnMessage(game.Gameplay.PlayerTurn);

            game.ProcessEvent(nextPlayerTurnMessage);

            game.Gameplay.PlayerTurn.Should().Be(nextPlayer);
        }

        private Player CreatePlayer()
        {
            string id = Guid.NewGuid().ToString();
            Lobby.AddPlayer(id);
            Lobby.SetPlayerName(id, "Dr. Who");

            return Lobby.GetPlayer(id);
        }

        private Game CreateGame(Player player)
        {
            var game = new Game(player);
            return game;
        }

        private Game CreateAndStartGame(int playersCount = 4)
        {
            var player = CreatePlayer();
            var game = CreateGame(player);

            for (int i = 0; i < playersCount - 1; i++)
            {
                game.JoinPlayer(CreatePlayer());
            }

            game.Start();

            return game;
        }
    }
}