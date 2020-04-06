using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Messages
{
    [Serializable]
    public class GetGamesMessage : Message
    {
        public List<Game.Game> Games;
    }
}
