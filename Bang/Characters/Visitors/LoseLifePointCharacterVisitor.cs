using System;
using Bang.Players;

namespace Bang.Characters.Visitors
{
    public class LoseLifePointCharacterVisitor : ICharacterVisitor<Action<Player, byte>>
    {
        public Action<Player, byte> DefaultValue => (player, cardsAmount) => { };

        public Action<Player, byte> Visit(BartCassidy character)
        {
            return (player, cardsAmount) => player.TakeCards(cardsAmount);
        }
    }
}