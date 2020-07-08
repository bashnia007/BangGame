using System;
using Bang.Characters.Visitors;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game
{
    abstract class DefenceStrategy
    {
        private readonly CardType expectedCard;
        private readonly Player hitter;
        private readonly int requiredCards;
        
        protected DefenceStrategy(CardType expectedCard, Player hitter, int requiredCards)
        {
            this.expectedCard = expectedCard ?? throw new ArgumentNullException(nameof(expectedCard));
            
            if (requiredCards != 1 && requiredCards != 2) throw new ArgumentOutOfRangeException(nameof(requiredCards));
            
            this.requiredCards = requiredCards;
            this.hitter = hitter;
        }

        public bool Apply(Player defender, BangGameCard firstCard, BangGameCard secondCard = null)
        {
            if (defender == null) throw new ArgumentNullException(nameof(defender));

            Func<BangGameCard, CardType, bool> isValidCard = GetValidator(defender);

            bool saveLifePoint =
                requiredCards == 1 && isValidCard(firstCard, expectedCard) ||
                requiredCards == 2 && isValidCard(firstCard, expectedCard) && isValidCard(secondCard, expectedCard);

            if (saveLifePoint)
            {
                defender.DropCard(firstCard);
                if (requiredCards == 2)
                    defender.DropCard(secondCard);
            }
            else
            {
                defender.LoseLifePoint(hitter);
            }

            return saveLifePoint;
        }

        private Func<BangGameCard, CardType, bool> GetValidator(Player player)
        {
            return player.Character.Accept(new CardValidationForCharacterVisitor());
        }
    }
}