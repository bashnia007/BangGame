using Bang.PlayingCards;
using System;

namespace Bang.Characters.Visitors
{
    public class CardValidationForCharacterVisitor : ICharacterVisitor<Func<BangGameCard, CardType, bool>>
    {
        public Func<BangGameCard, CardType, bool> DefaultValue 
            => (card, expectedCard) 
            => { return card == expectedCard; };

        public Func<BangGameCard, CardType, bool> Visit(CalamityJanet character)
        {
            return (card, expectedCard) => 
            {
                return (card == expectedCard) 
                    || ((expectedCard == new BangCardType() || expectedCard == new MissedCardType()) 
                        && (card == new BangCardType() || card == new MissedCardType()));
            };
        }
    }
}
