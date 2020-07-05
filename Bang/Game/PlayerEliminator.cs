using System;
using System.Collections.Generic;
using Bang.Characters.Visitors;
using Bang.Players;
using Bang.Roles;

namespace Bang.Game
{
    static class PlayerEliminator
    {
        internal static void Eliminate(Player victim, Player responsible, IReadOnlyList<Player> alivePlayers)
        {
            if (victim == null)
                throw new ArgumentNullException(nameof(victim));
            
            if (victim.IsAlive)
                throw new ArgumentException($"Player {victim.Name} is still alive!");
            
            if (alivePlayers == null)
                throw new ArgumentNullException(nameof(alivePlayers));

            var stealVictimCards = new VultureCharacterVisitor();
            foreach (var alivePlayer in alivePlayers)
            {
                var func = alivePlayer.Character.Accept(stealVictimCards);
                func(new VultureInfo{ Victim = victim, Vulture = alivePlayer});
            }

            if (responsible != null && responsible != victim)
            {
                switch (victim.Role)
                {
                    // Any player eliminating an Outlaw must draw a reward of 3 cards from the deck.
                    case Outlaw _:
                        responsible.TakeCards(3);
                        break;

                    // If the Sheriff eliminates a Deputy, the Sheriff must discard all the cards he has in hand and in play.
                    case Deputy _:
                        if (responsible.Role is Sheriff) responsible.DropAllCards();
                        break;
                }
            }
            
            victim.DropAllCards();
        }
    }
}