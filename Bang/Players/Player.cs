using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.PlayingCards;
using Bang.Roles;
using Bang.GameEvents;
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
        public Character Character => PlayerTablet.Character;
        public int LifePoints => PlayerTablet.Health;
        public bool IsAlive => PlayerTablet.IsAlive;

        private Game.Gameplay gamePlay;

        public Player()
        {
            hand = new List<BangGameCard>();
        }

        // TODO it would be much better add constructor with this parameters
        public void SetInfo(Game.Gameplay gamePlay, Role role, Character character)
        {
            this.gamePlay = gamePlay?? throw new ArgumentNullException();
            Role = role?? throw new ArgumentNullException();
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }

        public void AddCardToHand(BangGameCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            
            hand.Add(card);
        }

        public void DropActiveCard(BangGameCard card)
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

        public void TakeCards(byte amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var newCard = gamePlay.DealCard();
                hand.Add(newCard);
            }
        }

        public void Defense(BangGameCard firstCard, BangGameCard secondCard = null)
        {
            if (firstCard == null)
            {
                NotDefense();
                return;
            }
            
            if (!hand.Contains(firstCard))
                throw new InvalidOperationException($"Player doesn't have card {firstCard.Description}");
            
            if (secondCard != null && !hand.Contains(secondCard))
                throw new InvalidOperationException($"Player doesn't have card {secondCard.Description}");

            gamePlay.Defense(this, firstCard, secondCard);
        }

        public void NotDefense()
        {
            gamePlay.Defense(this, null);
        }

        public Response PlayCard(BangGameCard card, Player playOn = null)
        {
            if (!hand.Contains(card))
                throw new InvalidOperationException($"Player doesn't have card {card.Description}");

            if (card.CanBePlayedToAnotherPlayer)
            {
                if (playOn == null || playOn == this)
                    throw new InvalidOperationException($"Card {card.Description} must be played to another player!");
            }
            else
            {
                if (playOn != null && playOn != this)
                    throw new InvalidOperationException($"Card {card.Description} can not be played to another player!");
            }

            var response = gamePlay.CardPlayed(playOn?? this, card);

            if (response is LeaveCardOnTheTableResponse)
            {
                hand.Remove(card);
                return response;
            }

            if (!(response is NotAllowedOperation))
                DropCard(card);

            return response;
        }

        public void ForceToDropCard(BangGameCard card)
        {
            gamePlay.ForceDropCard(card);
        }

        public void ForceToDropRandomCard()
        {
            gamePlay.ForceDropRandomCard();
        }

        public void RegainLifePoint()
        {
            PlayerTablet.Health++;
        }

        public void LoseLifePoint(int loseLifeAmount = 1)
        {
            if(loseLifeAmount <= 0)
                throw new ArgumentOutOfRangeException();
            
            if (!IsAlive)
                throw new InvalidOperationException($"Player {Name} is already dead");

            for (int i = 0; i < loseLifeAmount; i++)
            {
                PlayerTablet.Health--;
            }

            if (IsAlive)
            {
                var visitor = new LoseLifePointCharacterVisitor();
                var action = Character.Accept(visitor);
                
                action(this, (byte) loseLifeAmount);
            }
        }
        
        public void LoseCard(BangGameCard card)
        {
            hand.Remove(card);
        }

        public void ChooseCard(BangGameCard card)
        {
            gamePlay.ChooseCard(card, this);
        }

        public void DrawCardFromPlayer(BangGameCard card) => gamePlay.StealCard(card);
        public void DrawCardFromPlayer() => gamePlay.StealCard();
    }
}
