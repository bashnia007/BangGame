using Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Server
{
    class Client
    {
        public delegate void RemoveConnectionHandler(string id);
        public delegate void AddConnectionHandler(Client client);
        public event RemoveConnectionHandler RemoveConnection;
        public event AddConnectionHandler AddConnection;

        public string Id { get; }
        private NetworkStream _stream { get; set; }
        private TcpClient _client;

        public Client(TcpClient tcpClient)
        {
            Id = Guid.NewGuid().ToString();
            _client = tcpClient;

            AddConnection?.Invoke(this);
        }

        public void Process()
        {
            try
            {
                _stream = _client.GetStream();

                // receiving messages from client
                while (true)
                {
                    var message = GetMessage();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                RemoveConnection?.Invoke(Id);
                Close();
            }
        }

        private Message GetMessage()
        {
            IFormatter formatter = new BinaryFormatter();
            Message message = (Message)formatter.Deserialize(_stream);

            return message;
        }

        public void Close()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}
