using System;
using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.Exceptions;
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
            if (firstCard != null && !defender.Hand.Contains(firstCard))
                throw new PlayerDoesntHaveSuchCardException(defender, firstCard);
            if (secondCard != null && !defender.Hand.Contains(secondCard))
                throw new PlayerDoesntHaveSuchCardException(defender, secondCard);

            if (defender.Character is SuzyLafayette && requiredCards == 2 && defender.Hand.Count == 1)
            {
                defender.TakeCards(1);
                secondCard = defender.Hand.First(c => c != firstCard);
            }

            Func<BangGameCard, CardType, bool> isValidCard = GetValidator(defender);

            bool saveLifePoint =
                requiredCards == 1 && isValidCard(firstCard, expectedCard) ||
                requiredCards == 2 && isValidCard(firstCard, expectedCard) && isValidCard(secondCard, expectedCard);

            if (saveLifePoint)
            {
                defender.DropPlayedCard(firstCard);
                if (requiredCards == 2)
                    defender.DropPlayedCard(secondCard);
            }
            else
            {
                if (defender.Character is SuzyLafayette && requiredCards == 2)
                {
                    defender.DropPlayedCard(firstCard);
                }
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