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
    public class PlayerTablet
    {
        public readonly int MaximumHealth;

        private int _health;
        public int Health
        {
            get => _health;
            set => _health = Math.Min(value, MaximumHealth);
        }

        public bool IsAlive => Health > 0;
        public Weapon Weapon { get; private set; }
        public Character Character { get; }
        private readonly List<LongTermFeatureCard> _cards = new List<LongTermFeatureCard>();
        public ReadOnlyCollection<LongTermFeatureCard> LongTermFeatureCards => _cards.AsReadOnly();
        public bool IsSheriff { get; }

        public event EventHandler<LongTermFeatureCard> CardDropped;

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
        public bool CanPutCard(LongTermFeatureCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            if (card is WeaponCard)
            {
                return !_cards.OfType<WeaponCard>().Any();
            }
            
            return !_cards.Exists(c => c == card);
        }
        
        /// <summary>
        /// Adds card to playing board
        /// </summary>
        /// <param name="card"></param>
        /// <exception cref="ArgumentNullException">generates if card is null</exception>
        /// <exception cref="DuplicatedCardException">card is already in play</exception>
        public void PutCard(LongTermFeatureCard card)
        {
            if (!CanPutCard(card))
                throw new DuplicatedCardException(card.Description); 
            
            if (card is WeaponCard weaponCard)
            {
                Weapon = WeaponFactory.Create(weaponCard);
            }
            
            _cards.Add(card);
        }

        /// <summary>
        /// Removes card from playing board
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(LongTermFeatureCard card)
        {
            if (card is WeaponCard)
            {
                Weapon = WeaponFactory.DefaultWeapon;
            }

            _cards.Remove(card);
            
            OnCardDropped(card);
        }

        private void OnCardDropped(LongTermFeatureCard card)
        {
            CardDropped?.Invoke(this, card);
        }
    }
}
