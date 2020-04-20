using Domain.Characters;
using Domain.PlayingCards;
using Domain.Roles;

using System;
using System.Collections.Generic;


namespace Domain.Players
{
    [Serializable]
    public abstract class Player
    {
        public string Id { get; protected set; }
        public string Name { get; set; }
        public Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        public List<BangGameCard> PlayerHand { get; private set; }
        public virtual bool IsReadyToPlay { get; set; }

        public delegate void DropCardsHandler(List<BangGameCard> cardsToDrop);
        public delegate List<BangGameCard> TakeCardsHandler(short amount, string playerId);
        public event DropCardsHandler CardsDropped;
        public event TakeCardsHandler CardsTaken;

        public Player()
        {
            PlayerHand = new List<BangGameCard>();
        }

        public void SetInfo(Role role, Character character)
        {
            Role = role;
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }

        public void DropCards(List<BangGameCard> cardsToDrop)
        {
            CardsDropped?.Invoke(cardsToDrop);

            foreach (var card in cardsToDrop)
            {
                PlayerHand.Remove(card);
            }
        }

        public void DropCard(BangGameCard cardToDrop)
        {
            DropCards(new List<BangGameCard> { cardToDrop });
        }

        public List<BangGameCard> TakeCards(short amount)
        {
            var newCards = CardsTaken?.Invoke(amount, Id);
            PlayerHand.AddRange(newCards);

            return newCards;
        }
    }
}
