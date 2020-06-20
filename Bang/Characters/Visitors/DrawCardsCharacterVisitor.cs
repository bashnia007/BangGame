using Bang.Game;
using Bang.Players;
using System;

namespace Bang.Characters.Visitors
{
    public class DrawCardsCharacterVisitor : ICharacterVisitor<Action<Game.Gameplay, Player>>
    {
        public Action<Game.Gameplay, Player> DefaultValue => (gameplay, player) => ProvideTwoCards(gameplay, player);

        public Action<Game.Gameplay, Player> Visit(BlackJack character)
        {
            return (gameplay, player) =>
            {
                var blackJackChecker = new BlackJackCharacterChecker();

                player.AddCardToHand(gameplay.DealCard());
                if (blackJackChecker.Peek(gameplay))
                {
                    player.AddCardToHand(gameplay.DealCard());
                }
                player.AddCardToHand(gameplay.DealCard());
            };
        }

        private void ProvideTwoCards(Game.Gameplay gameplay, Player player)
        {
            for (int i = 0; i < 2; i++)
            {
                player.AddCardToHand(gameplay.DealCard());
            }
        }
    }
}
