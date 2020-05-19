using Bang.Players;

namespace Bang.GameEvents
{
    public class CheckDrawCardMessage : BangGameMessage
    {
        public override Response Handle(BangEventsHandler handler)
        {
            return handler.CheckDrawCard(this);
        }

        public CheckDrawCardMessage(Player player) : base(player)
        {
        }
    }
}