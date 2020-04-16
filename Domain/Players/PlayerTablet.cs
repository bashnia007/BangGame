using Domain.Characters;
using Domain.Exceptions;
using Domain.PlayingCards;
using Domain.Weapons;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace Domain.Players
{
    
    /// <summary>
    /// Describes all VISIBLE FOR EVERYONE information about player
    /// </summary>
    [Serializable]
    public class PlayerTablet
    {
        public readonly int MaximumHealth;

        private int health;
        public int Health
        {
            get => health;
            set => health = Math.Min(value, MaximumHealth);
        }

        public bool IsAlive => Health > 0;
        public Weapon Weapon { get; private set; }
        public Character Character { get; }
        
        private readonly List<BangGameCard> activeCards = new List<BangGameCard>();
        public IReadOnlyList<BangGameCard> ActiveCards => activeCards;
        public bool IsSheriff { get; }

        public event Player.DropCardsHandler CardDropped;

        public PlayerTablet(Character character, bool isSheriff)
        {
            Character = character;
            IsSheriff = isSheriff;
            MaximumHealth = isSheriff ? Character.LifePoints + 1 : Character.LifePoints;
            Health = MaximumHealth;
            Weapon = WeaponFactory.DefaultWeapon;
        }
        
        /// <summary>
        /// Checks if player can add card to playing board
        /// Player can have:
        ///     only 1 copy of any one card in play;
        ///     only 1 weapon in play
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">card is null</exception>
        public bool CanPutCard(BangGameCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            if (!card.IsLongTermCard) return false;

            if (card.IsWeaponCard)
            {
                return !activeCards.Any(c => c.IsWeaponCard);
            }
            
            return !activeCards.Exists(c => c.Type == card.Type);
        }
        
        /// <summary>
        /// Adds card to playing board
        /// </summary>
        /// <param name="card"></param>
        /// <exception cref="ArgumentNullException">generates if card is null</exception>
        /// <exception cref="DuplicatedCardException">card is already in play</exception>
        public void PutCard(BangGameCard card)
        {
            if (!CanPutCard(card))
                throw new DuplicatedCardException(card.Description); 
            
            if (card.IsWeaponCard)
            {
                Weapon = WeaponFactory.Create(card.Type as WeaponCardType);
            }
            
            activeCards.Add(card);
        }

        /// <summary>
        /// Removes card from playing board
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(BangGameCard card)
        {
            if (card.Type is WeaponCardType)
            {
                Weapon = WeaponFactory.DefaultWeapon;
            }

            activeCards.Remove(card);
            
            OnCardDropped(card);
        }

        private void OnCardDropped(BangGameCard card)
        {
            CardDropped?.Invoke(new List<BangGameCard>{card});
        }
    }
}
