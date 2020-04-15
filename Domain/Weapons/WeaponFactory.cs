using System;
using Domain.PlayingCards;
using Domain.PlayingCards.Visitors;

namespace Domain.Weapons
{
    public static class WeaponFactory
    {
        public static readonly Weapon DefaultWeapon = new Colt(); 
        
        public static Weapon Create(WeaponCardType card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            var visitor = new LongTermCardTypeToWeaponMatcher();
            var weapon = card.Accept(visitor);
            
            return weapon?? DefaultWeapon;
        }
    }
}