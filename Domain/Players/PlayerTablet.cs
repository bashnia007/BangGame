using System;
using Domain.PlayingCards;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Domain.Exceptions;

namespace Domain.Players
{
    /// <summary>
    /// Describes all VISIBLE FOR EVERYONE information about player
    /// </summary>
    public class PlayerTablet
    {
        public int Health { get; set; }
        public bool IsAlive => Health > 0;
        public Weapon Weapon { get; private set; }
        public Character.Character Character { get; }
        private readonly List<LongTermFeatureCard> _cards = new List<LongTermFeatureCard>();
        public ReadOnlyCollection<LongTermFeatureCard> LongTermFeatureCards => _cards.AsReadOnly();
        public bool IsSheriff { get; }

        public PlayerTablet(Character.Character character, bool isSheriff)
        {
            Character = character;
            IsSheriff = isSheriff;
            Health = isSheriff ? Character.LifePoints + 1 : Character.LifePoints;
        }
        
        /// <summary>
        /// Checks if player can play card. Player can have only 1 copy of any one card in play;
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">card is null</exception>
        public bool CanPutCard(LongTermFeatureCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            return card is WeaponCard || _cards.Any(c => c != card);
        }
        
        // TODO Ideas for tests:
        // When player put weapon card, previous weapon card dropped
        // if player doesn't have a weapon card, his weapon is colt
        // 
        
        /// <summary>
        /// Adds card in play
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
                var previousWeaponCard = _cards.OfType<WeaponCard>().FirstOrDefault();
                
                if (previousWeaponCard != null)
                {
                    RemoveCard(previousWeaponCard);
                }

                Weapon = WeaponFactory.Create(weaponCard);
            }
            
            _cards.Add(card);
        }

        /// <summary>
        /// Removes card from tablet
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(LongTermFeatureCard card)
        {
            if (card is WeaponCard)
            {
                Weapon = WeaponFactory.DefaultWeapon;
            }
            
            // TODO drop card

            _cards.Remove(card);
        }
    }
}
