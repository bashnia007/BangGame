using NLog;
using System;
using System.Threading;

namespace Server
{
    class Program
    {
        private static Server _server;
        private static Thread _listenThread;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                Logger.Debug("Server started");
                _server = new Server();
                _listenThread = new Thread(_server.Listen);
                _listenThread.Start(); 
            }
            catch (Exception ex)
            {
                Logger.Fatal("Server stopped due to exception: " + ex.Message);
                Console.WriteLine(ex.Message);
                if (_server != null)
                {
                    _server.StopServer();
                }
                if (_listenThread != null)
                {
                    _listenThread.Abort();
                }
            }
        }
    }
}
