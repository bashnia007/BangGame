using Bang.GameEvents;
using Bang.Messages;
using Bang.Players;

namespace Server.Messages
{
    public class NextPlayerTurnMessage : BangGameMessage
    {
        public NextPlayerTurnMessage(Player player) : base(player)
        {

        }
        public override Response Handle(BangEventsHandler handler)
        {
            return Player.EndTurn();
        }
    }
}
