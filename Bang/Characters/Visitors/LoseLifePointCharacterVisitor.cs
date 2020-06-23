using System;
using Bang.Players;

namespace Bang.Characters.Visitors
{
    public class LoseLifePointCharacterVisitor : ICharacterVisitor<Action<Player>>
    {
        public Action<Player> DefaultValue
        {
            get
            {
                return (player) => { };
            }
        }

        public Action<Player> Visit(BartCassidy character)
        {
            return (player) => player.TakeCards(1);
        }
    }
}