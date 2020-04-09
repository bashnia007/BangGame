using System;
using System.Collections.Generic;

namespace Domain.Messages
{
    [Serializable]
    public class GetGamesMessage : Message
    {
        public List<Game.Game> Games;

        public override void Accept(IMessageProcessor visitor)
        {
            visitor.ProcessGetGamesMessage(this);
        }
    }
}
