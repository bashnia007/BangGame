using Domain.TCP;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private TcpListener _tcpListener; 
        private readonly BlockingCollection<Client> _clients = new BlockingCollection<Client>();
        private const int IntervalToCheckConnections = 10 * 1000;
        
        public void Listen()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = IntervalToCheckConnections;
            timer.Elapsed += CheckClientConnections;
            timer.Start();

            try
            {
                _tcpListener = new TcpListener(IPAddress.Parse(TcpConfig.Ip), TcpConfig.Port);
                _tcpListener.Start();
                Console.WriteLine("Server started. Waiting for connections...");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                    Client client = new Client(tcpClient);
                    client.AddConnection += AddConnection;
                    client.RemoveConnection += RemoveConnection;

                    Thread clientThread = new Thread(client.Process);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                StopServer();
                timer.Stop();
            }
        }

        private void CheckClientConnections(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                foreach (var client in _clients.ToArray())
                {
                    if (!client.TcpClient.Connected)
                    {
                        RemoveConnection(client.Id);

                    }
                }
            });
        }

        public void StopServer()
        {
            _tcpListener.Stop(); 

            for (int i = 0; i < _clients.Count; i++)
            {
                _clients.ToArray()[i].Close(); 
            }
            Environment.Exit(0); 
        }

        public BlockingCollection<Client> GetConnectedClients()
        {
            return _clients;
        }
        
        private void AddConnection(Client client)
        {
            Console.WriteLine($"Client {client.Id} connected");
            _clients.TryAdd(client);
        }

        private void RemoveConnection(string id)
        {
            Client client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Console.WriteLine($"Client {client.Id} disconnected");
                _clients.TryTake(out client);
            }
        }
    }
}
