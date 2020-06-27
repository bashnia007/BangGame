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
                if (card != new BangCardType() && card != new MissedCardType())
                {
                    throw new ArgumentException("Only Bang or Missed card expected. But was " + card.Description);
                }

                if (expectedCard is BangCardType)
                {
                    return new BangGameCard(new BangCardType(), card.Suite, card.Rank); ;
                }
                else
                {
                    return new BangGameCard(new MissedCardType(), card.Suite, card.Rank);
                }
            };
        }
    }
}
