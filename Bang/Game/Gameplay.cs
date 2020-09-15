using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bang.Characters;
using Bang.Exceptions;
using Bang.Game.Phases;
using Bang.GameEvents;
using Bang.GameEvents.Enums;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.Game
{
    [Serializable]
    public class Gameplay
    {
        private Deck<BangGameCard> deck;
        private Deck<BangGameCard> discardedCards;
        private Deck<Character> characters;
        public IReadOnlyList<Player> Players { get; private set; } 

        private BangEventsHandler handler;

        private PhaseTransition phaseTransition;
        public Player PlayerTurn => phaseTransition.PlayerTurn;

        public IReadOnlyList<Player> AlivePlayers => Players.Where(p => p.IsAlive).ToList();

        public Gameplay(Deck<Character> characters, Deck<BangGameCard> gameCards)
        {
            deck = gameCards ?? throw new ArgumentNullException(nameof(gameCards));
            discardedCards = new Deck<BangGameCard>();

            this.characters = characters;
        }

        public void Initialize(List<Player> players)
        {
            handler = new BangEventsHandler(this);
            
            Players = players;
            ProvideCardsForPlayers(characters);
            phaseTransition = new PhaseTransition(this, players.First(p => p.PlayerTablet.IsSheriff));
        }

        internal void DealFirstCards()
        {
            deck.Shuffle();

            foreach (var player in Players)
                FillPlayerHand(player);
        }
        
        /// <summary>
        /// Return the top card from the discarded without removing it. Use it to check the top card only
        /// </summary>
        /// <returns></returns>
        public BangGameCard PeekTopCardFromDiscarded()
        {
            if (discardedCards.IsEmpty()) return null;
            return discardedCards.Peek();
        }

        /// <summary>
        /// Return the top card from the discarded deck
        /// </summary>
        /// <returns></returns>
        internal BangGameCard DealCardFromDiscarded()
        {
            if (discardedCards.IsEmpty()) return null;
            return discardedCards.Deal();
        }

        /// <summary>
        /// Return the top card from the deck without removing it. Use it to check the top card only
        /// </summary>
        /// <returns></returns>
        public BangGameCard PeekTopCardFromDeck()
        {
            if (deck.IsEmpty()) ResetDeck();

            return deck.Peek();
        }

        /// <summary>
        /// Return the top card from the deck
        /// </summary>
        /// <returns></returns>
        public BangGameCard DealCard()
        {
            if (deck.IsEmpty()) ResetDeck();
            
            return deck.Deal();
        }

        /// <summary>
        /// Drops card as part of discard phase.
        /// </summary>
        /// <param name="card"></param>
        public void DropCard(BangGameCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            var discardPhase = phaseTransition.ToDiscardPhase();
            
            if (!discardPhase.IsSuccess)
                throw new InvalidOperationException(discardPhase.Reason);
            
            discardPhase.Value.Do(card);
        }

        public void DropCardsFromHand(List<BangGameCard> cardsToDrop)
        {
            foreach (var card in cardsToDrop)
            {
                DropCard(card);
            }
        }

        public void PutCardOnDeck(BangGameCard card)
        {
            deck.Put(card);
        }

        private void ProvideCardsForPlayers(Deck<Character> characters)
        {
            var roles = new Deck<Role>(GamePlayInitializer.CreateRolesForGame(Players.Count));
            characters.Shuffle();

            foreach (var player in Players)
            {
                player.SetInfo(this, roles.Deal(), characters.Deal());
            }
        }

        internal Response CardPlayed(Player player, BangGameCard card, Player target = null)
        {
            if (!player.Hand.Contains(card))
                return new NotAllowedOperation($"Player {player.Name} doesn't have card ${card}");
            
            if (!card.IsUniversalCard)
            {
                if (card.CanBePlayedToAnotherPlayer)
                {
                    if (target == null || player == target)
                        throw new InvalidOperationException($"Card {card.Description} must be played to another player!");
                }
                else
                {
                    if (target != null && player != target)
                        throw new InvalidOperationException($"Card {card.Description} can not be played to another player!");
                }
            }

            var playCardsPhaseResult = phaseTransition.ToPlayCardsPhase();
            
            if (!playCardsPhaseResult.IsSuccess)
                return new NotAllowedOperation(playCardsPhaseResult.Reason);

            var phase = playCardsPhaseResult.Value;

            var response = phase.ApplyCardEffect(card, target);

            if (response is NotAllowedOperation)
            {
                return response;
            }
            else if (response is LeaveCardOnTheTableResponse)
            {
                player.LoseCard(card);
                return response;
            }
            else
            {
                DropPlayedCard(player, card);
            }

            if (!(response is DefenceAgainstDuel))
            {
                CheckSuzyHand(PlayerTurn, 0);
            }
            
            return response;
        }

        internal Response CardPlayed(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return CardPlayed(player, firstCard); 
            
            if (!player.Hand.Contains(firstCard))
                throw new PlayerDoesntHaveSuchCardException(player, firstCard);
            
            if (!player.Hand.Contains(secondCard))
                throw new PlayerDoesntHaveSuchCardException(player, secondCard);
            
            if (player.Character != new SidKetchum())
                return new NotAllowedOperation();
            
            if (player.LifePoints == player.MaximumLifePoints)
                return new NotAllowedOperation();
            
            DropPlayedCard(player, firstCard);
            DropPlayedCard(player, secondCard);
            
            player.RegainLifePoint();
            
            return new Done();
        }

        private void FillPlayerHand(Player player)
        {
            while (player.PlayerTablet.Health > player.Hand.Count)
            {
                player.AddCardToHand(DealCard());
            }
        }

        private void ResetDeck()
        {
            deck = discardedCards.Shuffle();
            discardedCards = new Deck<BangGameCard>();
        }

        public Response Handle(BangGameMessage gameEvent)
        {
            Response response = gameEvent.Handle(handler);
            response.ReplyTo = gameEvent;
            
            return response;
        }

        /// <summary>
        /// Puts card to the discard pile
        /// </summary>
        /// <param name="card"></param>
        public void Discard(BangGameCard card)
        {
            discardedCards.Put(card);
        }

        public Response EndTurn()
        {
            var result1 = phaseTransition.ToDiscardPhase();
            if (!result1.IsSuccess)
                return new NotAllowedOperation(result1.Reason);
            
            var result2 = phaseTransition.ToDrawCardsPhase();
            return result2.IsSuccess? (Response) new Done() : new NotAllowedOperation(result2.Reason);
        }
        
        public Response StartPlayerTurn()
        {
            if (phaseTransition.DrawCardsPhase == null)
                throw new InvalidOperationException($"{nameof(DrawCardsPhase)} is null");
            
            if (!IsPlayerAliveAfterDynamite())
            {
                if (IsGameOver())
                {
                    var (team, winners) = FindWinners();
                    return new GameOverResponse(team, winners);
                }
            }

            if (!DoesPlayerLeaveJail())
            {
                phaseTransition.SkipTurn(this, GetNextPlayer());
                // TODO maybe it would be nicer to return response directly here
                return StartPlayerTurn();
            }

            return phaseTransition.DrawCardsPhase.Do();
        }

        private bool DoesPlayerLeaveJail()
        {
            var jailCard = PlayerTurn.ActiveCards.FirstOrDefault(c => c == new JailCardType());
            if (jailCard != null)
            {
                var jailChecker = new JailChecker();
                PlayerTurn.DiscardActiveCard(jailCard);

                if (!jailChecker.Draw(this, PlayerTurn.Character))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPlayerAliveAfterDynamite()
        {
            var dynamiteCard = PlayerTurn.ActiveCards.FirstOrDefault(c => c == new DynamiteCardType());
            if (dynamiteCard != null)
            {
                var dynamiteChecker = new DynamiteChecker();

                if (dynamiteChecker.Draw(this, PlayerTurn.Character))
                {
                    PlayerTurn.DiscardActiveCard(dynamiteCard);
                    // TODO introduce constant or maybe even enum
                    PlayerTurn.LoseLifePoint(3);
                    
                    // TODO move to StartNextPlayerTurn
                    if (!PlayerTurn.PlayerTablet.IsAlive)
                    {
                        return false;
                    }
                }
                else
                {
                    var nextPlayer = GetNextPlayer();
                    PlayerTurn.PlayerTablet.RemoveCard(dynamiteCard);
                    nextPlayer.PlayerTablet.PutCard(dynamiteCard);
                }
            }

            return true;
        }
        
        public Player GetNextPlayer()
        {
            int indexOfCurrentPlayer = AlivePlayers.ToList().IndexOf(PlayerTurn);

            return AlivePlayers[(indexOfCurrentPlayer + 1) % AlivePlayers.Count];
        }

        public Response ProcessReplyAction(Player victim)
        {
            var response = phaseTransition.PlayCardPhase.ApplyCardEffect(victim);
            
            CheckSuzyHand(victim, 0);
            return response;
        }

        public Response ProcessReplyAction(Player player, BangGameCard card)
        {
            Debug.Assert(phaseTransition.DrawCardsPhase != null || phaseTransition.PlayCardPhase != null);

            if (phaseTransition.DrawCardsPhase != null)
            {
                return phaseTransition.DrawCardsPhase.Reply(card);
            }

            var response = phaseTransition.PlayCardPhase.ApplyCardEffect(card, player);
            
            CheckSuzyHand(player, 0);

            if (response is Done)
            {
                if (IsGameOver())
                    return GameOverResponse();
            }
            
            return response;
        }
        
        public Response ProcessReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ProcessReplyAction(player, firstCard);
            if (phaseTransition.PlayCardPhase == null) throw new InvalidOperationException();

            return phaseTransition.PlayCardPhase.ApplyCardEffect(firstCard, secondCard, player);
        }

        public Response ProcessDrawSelection(DrawOptions drawOption)
        {
            return phaseTransition.DrawCardsPhase.ApplyDrawOption(drawOption);
        }

        private void CheckSuzyHand(Player player, int cardsLimit)
        {
            if (player.Character is SuzyLafayette && player.Hand.Count == cardsLimit)
                player.AddCardToHand(DealCard());
        }

        private GameOverResponse GameOverResponse()
        {
            Debug.Assert(IsGameOver());
                
            var (team, winners) = FindWinners();
            return new GameOverResponse(team, winners);
        }

        private bool IsGameOver()
        {
            if (!AlivePlayers.Any(p => p.Role is Sheriff))
            {
                return true;
            }

            if (AlivePlayers.All(p => !(p.Role is Renegade) && !(p.Role is Outlaw)))
            {
                return true;
            }

            return false;
        }

        private (Team, List<Player>) FindWinners()
        {
            Debug.Assert(IsGameOver());

            IEnumerable<Player> players;
            Team team;
            
            if (AlivePlayers.Any(p => p.Role is Sheriff))
            {
                team = Team.Sheriff;
                players = Players.Where(p => p.Role is Sheriff || p.Role is Deputy);
            }
            
            // Sheriff is dead

            // If Renegade is only alive then he will be winner
            else if (AlivePlayers.All(p => p.Role is Renegade))
            {
                team = Team.Renegade;
                players = Players.Where(p => p.Role is Renegade);
            }
            // otherwise outlaws are winner
            else
            {
                team = Team.Outlaws;
                players = Players.Where(p => p.Role is Outlaw);
            }

            return (team, players.ToList());
        }

        internal void DropPlayedCard(Player player, BangGameCard card)
        {
            player.LoseCard(card);
            discardedCards.Put(card);
        }
    }
}