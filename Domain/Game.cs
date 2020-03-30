using Domain.Players;
using System.Collections.Generic;

namespace Domain
{
    public class Game
    {
        public string Id { get; set; }
        public List<Player> Players { get; set; }
    }
}
