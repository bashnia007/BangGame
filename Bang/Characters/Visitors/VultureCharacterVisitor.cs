using System;
using System.Linq;

namespace Bang.Characters.Visitors
{
    internal class VultureCharacterVisitor : ICharacterVisitor<Action<VultureInfo>>
    {
        public Action<VultureInfo> DefaultValue => (vultureInfo) => { };

        public Action<VultureInfo> Visit(VultureSam character)
        {
            return (vultureInfo) =>
            {
                var victim = vultureInfo.Victim;
                var vulture = vultureInfo.Vulture;
                while (vultureInfo.Victim.Hand.Any())
                {
                    // maybe it will be cleaner to call vulture.DrawCardFromPlayer(victim)
                    var card = victim.Hand[0];
                    vulture.AddCardToHand(card);
                    victim.LoseCard(card);
                }

                while (victim.ActiveCards.Any())
                {
                    var card = victim.ActiveCards[0];
                    vulture.DrawPlayerActiveCard(victim, card);
                }
            };
        }
    }
}