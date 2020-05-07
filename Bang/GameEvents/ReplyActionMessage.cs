using System.Xml.Schema;
using Bang.Players;

namespace Bang.GameEvents
{
    public class ReplyActionMessage : BangGameMessage
    {
        public Response Response { get; set; }
        
        public ReplyActionMessage(Player player) : base(player)
        {
        }
        
        public override Response Handle(BangEventsHandler handler)
        {
            return handler.ReplyAction(this);
        }
    }
    
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