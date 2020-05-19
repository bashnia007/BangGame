using System;
using System.Collections.Generic;
using Server.Messages;

namespace Bang.Messages
{
    [Serializable]
    public class GetGamesMessage : Message
    {
        public List<Server.Game> Games;

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessGetGamesMessage(this);
        }
    }
}
