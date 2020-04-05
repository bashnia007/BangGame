using System;
using Domain.PlayingCards;
using Domain.PlayingCards.Visitors;

namespace Domain.Weapons
{
    public static class WeaponFactory
    {
        public static readonly Weapon DefaultWeapon = new Colt(); 
        
        public static Weapon Create(LongTermFeatureCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            
            var visitor = new LongTermCardToWeaponMatcher();
            var weapon = card.Accept(visitor);
            
            return weapon?? DefaultWeapon;
        }
    }
}