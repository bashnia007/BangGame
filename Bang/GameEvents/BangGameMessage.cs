using System.Diagnostics;
using Bang.Players;

namespace Bang.GameEvents
{
    public abstract class BangGameMessage
    {
        public Player Player { get; }

        public BangGameMessage(Player player)
        {
            Debug.Assert(player != null);

            Player = player;
        }
        public abstract Response Handle(BangEventsHandler handler);
    }
}