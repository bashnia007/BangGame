using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bang.Characters;
using Bang.PlayingCards;
using Bang.Roles;
using Bang.Game;
using Bang.GameEvents.CardEffects;

namespace Bang.Players
{
    [Serializable]
    public abstract class Player
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        private readonly List<BangGameCard> hand;
        public IReadOnlyList<BangGameCard> Hand => hand;
        public virtual bool IsReadyToPlay { get; set; }

        public IReadOnlyList<BangGameCard> ActiveCards => PlayerTablet.ActiveCards;
        public int LifePoints => PlayerTablet.Health;

        private Game.Gameplay gamePlay;

        public Player()
        {
            hand = new List<BangGameCard>();
        }

        // TODO it would be much better add constructor with this parameters
        public void SetInfo(Game.Gameplay gamePlay, Role role, Character character)
        {
            this.gamePlay = gamePlay;
            Role = role;
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }

        public void AddCardToHand(BangGameCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            
            hand.Add(card);
        }

        public void RemoveActiveCard(BangGameCard card)
        {
            PlayerTablet.RemoveCard(card);
            
            gamePlay.DropCard(card);
        }

        public void DropCards(List<BangGameCard> cardsToDrop)
        {
            gamePlay.DropCardsFromHand(cardsToDrop);
            foreach (var card in cardsToDrop)
            {
                hand.Remove(card);
            }
        }

        public void DropCard(BangGameCard cardToDrop)
        {
            DropCards(new List<BangGameCard> { cardToDrop });
        }

        public void TakeCards(short amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var newCard = gamePlay.DealCard();
                hand.Add(newCard);
            }
        }

        public void Defense(BangGameCard card)
        {
            if (card == null)
            {
                NotDefense();
                return;
            }
            
            if (!hand.Contains(card))
                throw new InvalidOperationException($"Player doesn't have card {card.Description}");

            gamePlay.Defense(this, card);
        }

        public void NotDefense()
        {
            gamePlay.Defense(this, null);
        }

        public void PlayCard(BangGameCard card, Player playOn = null)
        {
            if (!hand.Contains(card))
                throw new InvalidOperationException($"Player doesn't have card {card.Description}");
            
            if (card.CanBePlayedToAnotherPlayer && playOn == null)
                throw new InvalidOperationException($"Card {card.Description} must be played to another player!");

            bool played = gamePlay.CardPlayed(playOn?? this, card);
            if (played) DropCard(card);
        }

        public void ForceToDropCard(BangGameCard card)
        {
            gamePlay.ForceDropCard(card);
        }

        public void LoseLifePoint()
        {
            Debug.Assert(PlayerTablet.Health > 0);
            
            PlayerTablet.Health--;
        }
    }
}
