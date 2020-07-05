using System;
using System.Diagnostics;
using Bang.Players;

namespace Bang.GameEvents
{
    public abstract class BangGameMessage
    {
        public Player Player { get; }

        public BangGameMessage(Player player)
        {
            Player = player?? throw new ArgumentNullException(nameof(player));
        }
        public abstract Response Handle(BangEventsHandler handler);
    }
}