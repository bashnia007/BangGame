using System;
using Bang.Players;

namespace Bang.Characters.Visitors
{
    // TODO maybe it makes sense to introduce internal class DamageInfo { public byte Damage; public Player Hitter;} for more readability 
    internal class LoseLifePointCharacterVisitor : ICharacterVisitor<Action<Player, Player, byte>>
    {
        public Action<Player, Player, byte> DefaultValue => (player, hitter, cardsAmount) => { };

        public Action<Player, Player, byte> Visit(BartCassidy character)
        {
            return (victim, hitter, cardsAmount) => victim.TakeCards(cardsAmount);
        }

        public Action<Player, Player, byte> Visit(ElGringo character)
        {
            return (victim, hitter, cardsAmount) =>
            {
                if (hitter != null && victim != hitter)
                    victim.DrawCardFromPlayer(hitter);
            };
        }
    }
}