using Xunit;
using System.Net.Sockets;
using Domain.TCP;
using System.Threading;

namespace Server.Tests
{
    public class ServerTests
    {
        [Fact]
        public void ShouldAddTwoClient()
        {
            const int clientsAmount = 2;

            var server = new Server();
            var thread = new Thread(server.Listen);
            thread.Start();

            for (int i = 0; i < clientsAmount; i++)
            {
                var client = new TcpClient();
                client.Connect(TcpConfig.Ip, TcpConfig.Port);

                // we should wait till tcp connection establishes
                // it is a weak point, but right now I don't know other solution
                Thread.Sleep(15 * 1000);
            }
            
            Assert.Equal(clientsAmount, server.GetConnectedClients().Count);
        }

        [Fact]
        public void ShouldAddAndRemovePlayer()
        {
            var server = new Server();
            var thread = new Thread(server.Listen);
            thread.Start();

            var client = new TcpClient();
            client.Connect(TcpConfig.Ip, TcpConfig.Port);

            // we should wait till tcp connection establishes
            // it is a weak point, but right now I don't know other solution
            Thread.Sleep(15 * 1000);

            Assert.Single(server.GetConnectedClients());

            client.Close();

            Thread.Sleep(15 * 1000);
            Assert.Empty(server.GetConnectedClients());
        }
    }
}
