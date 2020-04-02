﻿using Domain.Messages;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    public class Client
    {
        public delegate void RemoveConnectionHandler(string id);
        public delegate void AddConnectionHandler(Client client);
        public event RemoveConnectionHandler RemoveConnection;
        public event AddConnectionHandler AddConnection;

        public string Id { get; }
        private NetworkStream _stream { get; set; }
        public TcpClient TcpClient;

        public Client(TcpClient tcpClient)
        {
            Id = Guid.NewGuid().ToString();
            TcpClient = tcpClient;
        }

        public void Process()
        {
            AddConnection?.Invoke(this);
            try
            {
                _stream = TcpClient.GetStream();

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
            TcpClient?.Close();
        }
    }
}