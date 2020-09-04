using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects;
using Bang.GameEvents.CardEffects.States;
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

        private HandlerState state;

        public Player PlayerTurn { get; private set; }
        public IReadOnlyList<Player> AlivePlayers => Players.Where(p => p.IsAlive).ToList();

        public Gameplay(Deck<Character> characters, Deck<BangGameCard> gameCards)
        {
            deck = gameCards ?? throw new ArgumentNullException(nameof(gameCards));
            discardedCards = new Deck<BangGameCard>();

            this.characters = characters;
            this.state = new DoneState(this);
        }

        public void Initialize(List<Player> players)
        {
            handler = new BangEventsHandler(this);
            
            Players = players;
            ProvideCardsForPlayers(characters);
            PlayerTurn = players.First(p => p.PlayerTablet.IsSheriff);
        }

        public void DealFirstCards()
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
        public BangGameCard DealCardFromDiscarded()
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

        public void DropCard(BangGameCard card) => discardedCards.Put(card);

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

        internal Response CardPlayed(Player currentPlayer, Player playOn, BangGameCard card)
        {
            var nextState = ApplyEffects(playOn, card);

            if (!nextState.IsError)
                state = nextState;

            CheckSuzyHand(currentPlayer, 1);

            return nextState.SideEffect;
        }

        private Response GivePhaseOneCards()
        {
            var nextState = PlayerTurn
                .Character
                .Accept(new DrawCardsCharacterVisitor())
                .Invoke(this, PlayerTurn);

            if (!nextState.IsError)
                state = nextState;

            return state.SideEffect;
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

        public void Discard(BangGameCard card)
        {
            discardedCards.Put(card);
        }

        public void StealCard(Player victim, BangGameCard card)
        {
            state = ApplyEffects(victim, card);
        }

        public void StealCard(Player victim)
        {
            state = ApplyEffects(victim);
        }

        public void ChooseCard(BangGameCard card, Player player)
        {
            state = ApplyEffects(player, card);
        }

        public Response StartPlayerTurn()
        {
            if (!IsPlayerAliveAfterDynamite())
            {
                if (IsGameOver())
                {
                    var (team, winners) = FindWinners();
                    state = new GameoverState(this, team, winners);
                    return state.SideEffect;
                }
            }

            if (!DoesPlayerLeaveJail())
            {
                NextTurn();
                return StartPlayerTurn();
            }

            return GivePhaseOneCards();
        }

        internal Response EndTurn()
        {
            if (!CanPassTurn(PlayerTurn))
                return new NotAllowedOperation($"{PlayerTurn.Name} exceeds hand-size limit");
            
            NextTurn();
            return new Done();
        }

        private bool DoesPlayerLeaveJail()
        {
            var jailCard = PlayerTurn.ActiveCards.FirstOrDefault(c => c == new JailCardType());
            if (jailCard != null)
            {
                var jailChecker = new JailChecker();
                PlayerTurn.DropActiveCard(jailCard);

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
                    PlayerTurn.DropActiveCard(dynamiteCard);
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

        private bool CanPassTurn(Player player)
        {
            return player.Hand.Count <= player.LifePoints;
        }

        // TODO make it internal or private 
        public void NextTurn()
        {
            state.BangAlreadyPlayed = false;
            PlayerTurn = GetNextPlayer();
        }

        public Player GetNextPlayer()
        {
            // TODO use AlivePlayers instead
            var playersAlive = Players.Where(p => p.PlayerTablet.IsAlive).ToList();
            int indexOfCurrentPlayer = playersAlive.IndexOf(PlayerTurn);

            return playersAlive[(indexOfCurrentPlayer + 1) % playersAlive.Count];
        }

        public Response ProcessReplyAction(Player victim)
        {
            state = state.ApplyCardEffect(victim);
            return state.SideEffect;
        }

        public Response ProcessReplyAction(Player player, BangGameCard card)
        {
            state = ApplyEffects(player, card);
            return state.SideEffect;
        }
        
        public Response ProcessReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ProcessReplyAction(player, firstCard);
            
            state = ApplyEffects(player, firstCard, secondCard);
            return state.SideEffect;
        }

        public Response ProcessDrawSelection(Player player, DrawOptions drawOption)
        {
            state = state.ApplyDrawOption(player, drawOption);
            return state.SideEffect;
        }

        private void CheckSuzyHand(Player player, int cardsLimit)
        {
            if (player.Character is SuzyLafayette && player.Hand.Count == cardsLimit && !(state is WaitingBangAfterDuelState))
                player.AddCardToHand(DealCard());
        }

        private HandlerState ApplyEffects(Player player)
        {
            state = state.ApplyCardEffect(player);

            state = UpdateStateIfGameOver(state);

            return state;
        }
        
        private HandlerState ApplyEffects(Player player, BangGameCard card)
        {
            state = state.ApplyCardEffect(player, card);
            
            CheckSuzyHand(player, 0);

            state = UpdateStateIfGameOver(state);

            return state;
        }
        
        private HandlerState ApplyEffects(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            state = state.ApplyCardEffect(player, firstCard, secondCard);

            CheckSuzyHand(player, 0);
            state = UpdateStateIfGameOver(state);

            return state;
        }

        private HandlerState UpdateStateIfGameOver(HandlerState state)
        {
            if (state.IsFinalState && IsGameOver())
            {
                var (team, winners) = FindWinners();
                return new GameoverState(this, team, winners);
            }

            return state;
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
    }
}