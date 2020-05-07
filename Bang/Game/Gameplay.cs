using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using Bang.GameEvents.CardEffects;

namespace Bang.Game
{
    [Serializable]
    public class Gameplay
    {
        private Deck<BangGameCard> deck;
        private Deck<BangGameCard> discardedCards;
        public IReadOnlyList<Player> Players { get; private set; } 

        private BangEventsHandler handler;

        private HandlerState state = new DoneState();
        public Queue<Response> innerEvents = new Queue<Response>();

        public Player PlayerTurn { get; private set; }
        
        public void Initialize(List<Player> players)
        {
            Players = players;
            
            deck = new Deck<BangGameCard>(GameInitializer.PlayingCards);
            deck.Shuffle();
            
            discardedCards = new Deck<BangGameCard>();
            
            ProvideCardsForPlayers();
            
            this.handler = new BangEventsHandler(this);
            PlayerTurn = players.First(p => p.PlayerTablet.IsSheriff);
        }

        public bool Defense(Player player, BangGameCard card)
        {
            state = state.ApplyReplyAction(card);

            return true;
        }

        public BangGameCard GetTopCardFromDiscarded()
        {
            if (discardedCards.IsEmpty()) return null;
            return discardedCards.Peek();
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

        private void ProvideCardsForPlayers()
        {
            var roles = new Deck<Role>(GameInitializer.CreateRolesForGame(Players.Count).Cast<Role>());
            var characters = new Deck<Character>(GameInitializer.Characters.Cast<Character>());

            foreach (var player in Players)
            {
                player.SetInfo(this, roles.Deal(), characters.Deal());
                FillPlayerHand(player);
            }
        }

        internal bool CardPlayed(Player player, BangGameCard card)
        {
            var nextState = state.ApplyCardEffect(player, card, this);
            
            if (!nextState.IsError)
            {
                state = nextState;
            }
            else
            {
                // TODO put message for game
            }
            
            return !nextState.IsError;
        }

        public void ForceDropCard(BangGameCard card)
        {
            state = state.ApplyReplyAction(card);
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
    }
}