using System;
using System.Collections.Generic;
using System.Linq;
using Bang.Characters;
using Bang.Exceptions;
using Bang.PlayingCards;
using Bang.PlayingCards.Visitors;
using Bang.Weapons;
using Bang.Game;

namespace Bang.Players
{
    
    /// <summary>
    /// Describes all VISIBLE FOR EVERYONE information about player
    /// </summary>
    [Serializable]
    public class PlayerTablet
    {
        private readonly Weapon DefaultWeapon = new Colt();
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

        public PlayerTablet(Character character, bool isSheriff)
        {
            Character = character;
            IsSheriff = isSheriff;
            
            MaximumHealth = isSheriff ? Character.LifePoints + 1 : Character.LifePoints;
            Health = MaximumHealth;
            Weapon = DefaultWeapon;
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

            if (!card.IsLongTerm) return false;

            if (card.IsWeapon)
            {
                return activeCards.All(c => !c.IsWeapon);
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
            
            if (card.IsWeapon)
            {
                Weapon = card.Accept(new CardToWeaponVisitor());
            }
            
            activeCards.Add(card);
        }

        /// <summary>
        /// Removes card from playing board
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(BangGameCard card)
        {
            if (card.IsWeapon)
            {
                Weapon = DefaultWeapon;
            }

            activeCards.Remove(card);
        }

        /// <summary>
        /// Put weapon on the tablet. If player has already weapon, previous is dropped
        /// </summary>
        /// <param name="card"></param>
        public void ChangeWeapon(BangGameCard card)
        {
            var previousWeapon = activeCards.FirstOrDefault(c => c.IsWeapon);
            
            if (previousWeapon != null)
            {
                RemoveCard(previousWeapon);
            }

            PutCard(card);
        }
    }
}
