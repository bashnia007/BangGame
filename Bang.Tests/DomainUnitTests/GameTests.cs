using Domain.Game;
using Domain.Players;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bang.Tests.DomainUnitTests
{
    public class GameTests
    {
        

        private List<Player> CreatePlayers(int amount)
        {
            var result = new List<Player>();

            for (int i = 0; i < amount; i++)
            {
                result.Add(new PlayerOnline());
            }

            return result;
        }
    }
}
