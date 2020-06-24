using System;
using Bang.Players;
using Bang.Roles;

namespace Bang.Game
{
    static class PlayerEliminator
    {
        internal static void Eliminate(Player victim, Player responsible)
        {
            if (victim == null)
                throw new ArgumentNullException(nameof(victim));
            
            if (victim.IsAlive)
                throw new ArgumentException($"Player {victim.Name} is still alive!");

            if (responsible != null && responsible != victim)
            {
                switch (victim.Role)
                {
                    // Any player eliminating an Outlaw must draw a reward of 3 cards from the deck.
                    case Outlaw o:
                        responsible.TakeCards(3);
                        break;

                    // If the Sheriff eliminates a Deputy, the Sheriff must discard all the cards he has in hand and in play.
                    case Deputy d:
                        if (responsible.Role is Sheriff s) responsible.DropAllCards();
                        break;
                }
            }
            
            victim.DropAllCards();
        }
    }
}