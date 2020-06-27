using Bang.PlayingCards;
using System;

namespace Bang.Characters.Visitors
{
    public class BangAsMissedCharacterVisitor : ICharacterVisitor<Func<BangGameCard, CardType, BangGameCard>>
    {
        public Func<BangGameCard, CardType, BangGameCard> DefaultValue 
            => (card, expectedCard) 
            => { return card; };

        public Func<BangGameCard, CardType, BangGameCard> Visit(CalamityJanet character)
        {
            return (card, expectedCard) => 
            {
                if (card == new BangCardType() || card == new MissedCardType())
                {
                    if (expectedCard == new BangCardType())
                    {
                        return new BangGameCard(new BangCardType(), card.Suite, card.Rank); ;
                    }
                    if (expectedCard == new MissedCardType())
                    {
                        return new BangGameCard(new MissedCardType(), card.Suite, card.Rank);
                    }
                }

                return card;
            };
        }
    }
}
