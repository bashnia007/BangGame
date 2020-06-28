using System;
using Bang.Players;

namespace Bang.Characters.Visitors
{
    internal class LoseLifePointCharacterVisitor : ICharacterVisitor<Action<Player, DamageInfo>>
    {
        public Action<Player, DamageInfo> DefaultValue => (player, damageInfo) => { };

        public Action<Player, DamageInfo> Visit(BartCassidy character)
        {
            return (victim, damageInfo) => victim.TakeCards(damageInfo.Damage);
        }

        public Action<Player, DamageInfo> Visit(ElGringo character)
        {
            return (victim, damageInfo) =>
            {
                if (damageInfo?.Hitter != null && victim != damageInfo.Hitter)
                    victim.DrawCardFromPlayer(damageInfo.Hitter);
            };
        }
    }
}