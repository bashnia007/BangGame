using System;
using System.Collections.Generic;
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
        
        public bool Defense(Player player, BangGameCard card)
        {
            state = state.ApplyCardEffect(player, card);

            if (player.Character is SuzyLafayette && player.Hand.Count == 0 && !(state is WaitingBangAfterDuelState))
                player.AddCardToHand(DealCard());

            return true;
        }

        public bool Defense(Player player, BangGameCard card, BangGameCard secondCard)
        {
            if (secondCard == null) return Defense(player, card);
            
            state = state.ApplyCardEffect(player, card, secondCard);

            CheckSuzyHand(player, 0);

            return true;
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
            var nextState = state.ApplyCardEffect(playOn, card);

            if (!nextState.IsError)
                state = nextState;

            CheckSuzyHand(currentPlayer, 1);

            return nextState.SideEffect;
        }

        public void ForceDropCard(Player victim, BangGameCard card)
        {
            state = state.ApplyCardEffect(victim, card);
        }

        public void ForceDropRandomCard(Player victim)
        {
            state = state.ApplyCardEffect(victim);
        }

        public Response GivePhaseOneCards()
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
            state = state.ApplyCardEffect(victim, card);
        }

        public void StealCard(Player victim)
        {
            state = state.ApplyCardEffect(victim);
        }

        public void ChooseCard(BangGameCard card, Player player)
        {
            state = state.ApplyCardEffect(player, card);
        }

        public Response StartNextPlayerTurn()
        {
            NextTurn();
            
            if (!IsPlayerAliveAfterDynamite() || !DoesPlayerLeaveJail()) return StartNextPlayerTurn();

            return GivePhaseOneCards();
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
            state = state.ApplyCardEffect(player, card);
            return state.SideEffect;
        }
        
        public Response ProcessReplyAction(Player player, BangGameCard firstCard, BangGameCard secondCard)
        {
            if (secondCard == null) return ProcessReplyAction(player, firstCard);
            
            state = state.ApplyCardEffect(player, firstCard, secondCard);
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
    }
}