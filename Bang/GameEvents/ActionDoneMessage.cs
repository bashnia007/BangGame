using Bang.Players;

namespace Bang.GameEvents
{
    public class ActionDoneMessage : BangGameMessage
    {
        public Response Response { get; set; }
        
        public ActionDoneMessage(Player player) : base(player)
        {
        }
        
        public override Response Handle(BangEventsHandler handler)
        {
            return new Done();
        }
    }
}