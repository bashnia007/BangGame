using Bang.Game;
using Bang.Players;
using System;

namespace Bang.Characters.Visitors
{
    public class DrawCardsCharacterVisitor : ICharacterVisitor<Action<Game.Gameplay, Player>>
    {
        public Action<Game.Gameplay, Player> Visit(BartCassidy character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

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

        public Action<Game.Gameplay, Player> Visit(CalamityJanet character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(ElGringo character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(JessyJones character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(Jourdonnais character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(KitCarlson character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(LuckyDuke character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(PaulRegret character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(PedroRamirez character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(RoseDoolan character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(SidKetchum character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(SlabTheKiller character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(SuzyLafayette character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(VultureSam character)
        {
            return (gameplay, player) => ProvideTwoCards(gameplay, player);
        }

        public Action<Game.Gameplay, Player> Visit(WillyTheKid character)
        {
            throw new NotImplementedException();
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
