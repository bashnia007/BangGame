using Domain.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        private TcpListener _tcpListener; 
        private readonly List<Client> _clients = new List<Client>(); 

        protected internal void AddConnection(Client client)
        {
            Console.WriteLine($"Client {client.Id} connected");
            _clients.Add(client);
        }

        protected internal void RemoveConnection(string id)
        {
            Client client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                Console.WriteLine($"Client {client.Id} disconnected");
                _clients.Remove(client);
            }
        }

        protected internal void Listen()
        {
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
            }
        }
        
        protected internal void StopServer()
        {
            _tcpListener.Stop(); 

            for (int i = 0; i < _clients.Count; i++)
            {
                _clients[i].Close(); 
            }
            Environment.Exit(0); 
        }
    }
}
