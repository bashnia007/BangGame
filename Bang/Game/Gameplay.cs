using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects.States;
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

        private HandlerState state = new DoneState();

        public Player PlayerTurn { get; private set; }

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
            PlayerTurn = players.First(p => p.PlayerTablet.IsSheriff);
        }

        public void DealFirstCards()
        {
            deck.Shuffle();

            foreach (var player in Players)
                FillPlayerHand(player);
        }

        public bool Defense(Player player, BangGameCard card, BangGameCard secondCard = null)
        {
            state = state.ApplyReplyAction(player, card, secondCard);

            return true;
        }

        public BangGameCard GetTopCardFromDiscarded()
        {
            if (discardedCards.IsEmpty()) return null;
            return discardedCards.Peek();
        }

        public BangGameCard GetTopCardFromDeck()
        {
            if (deck.IsEmpty()) ResetDeck();

            return deck.Peek();
        }

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

        private void ProvideCardsForPlayers(Deck<Character> characters)
        {
            var roles = new Deck<Role>(GamePlayInitializer.CreateRolesForGame(Players.Count));
            characters.Shuffle();

            foreach (var player in Players)
            {
                player.SetInfo(this, roles.Deal(), characters.Deal());
            }
        }

        internal Response CardPlayed(Player player, BangGameCard card)
        {
            var nextState = state.ApplyCardEffect(player, card, this);

            if (!nextState.IsError)
                state = nextState;
            
            return nextState.SideEffect;
        }

        public void ForceDropCard(BangGameCard card)
        {
            state = state.ApplyReplyAction(card);
        }

        public void ForceDropRandomCard()
        {
            state = state.ApplyReplyAction();
        }

        public void GivePhaseOneCards()
        {
            PlayerTurn
                .Character
                .Accept(new DrawCardsCharacterVisitor())
                .Invoke(this, PlayerTurn);
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

        public void StealCard(BangGameCard card)
        {
            state = state.ApplyReplyAction(card);
        }

        public void StealCard()
        {
            state = state.ApplyReplyAction();
        }

        public void ChooseCard(BangGameCard card, Player player)
        {
            state = state.ApplyReplyAction(player, card);
        }

        public void StartNextPlayerTurn()
        {
            SetNextPlayer();
            
            if (!IsPlayerAliveAfterDynamite() || !IsPlayerLeavesJail()) return;
            
            // todo provide 2 new cards 
        }

        private bool IsPlayerLeavesJail()
        {
            var jailCard = PlayerTurn.ActiveCards.FirstOrDefault(c => c == new JailCardType());
            if (jailCard != null)
            {
                var jailChecker = new JailChecker();
                PlayerTurn.DropActiveCard(jailCard);

                if (!jailChecker.Draw(this, PlayerTurn.Character))
                {
                    StartNextPlayerTurn();
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
                    if (!PlayerTurn.PlayerTablet.IsAlive)
                    {
                        StartNextPlayerTurn();
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

        public void SetNextPlayer()
        {
            PlayerTurn = GetNextPlayer();
        }

        public Player GetNextPlayer()
        {
            var playersAlive = Players.Where(p => p.PlayerTablet.IsAlive).ToList();
            int indexOfCurrentPlayer = playersAlive.IndexOf(PlayerTurn);

            return playersAlive[(indexOfCurrentPlayer + 1) % playersAlive.Count];
        }
    }
}