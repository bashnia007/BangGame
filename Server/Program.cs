using Domain.Game;
using System;
using System.Threading;

namespace Server
{
    class Program
    {
        private static Server _server;
        private static Thread _listenThread;

        static void Test()
        {
            GameInitializer gameInitializer = new GameInitializer();
            gameInitializer.CreateGameSet();
        }

        static void Main(string[] args)
        {
            Test();
            /*try
            {
                _server = new Server();
                _listenThread = new Thread(_server.Listen);
                _listenThread.Start(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (_server != null)
                {
                    _server.StopServer();
                }
                if (_listenThread != null)
                {
                    _listenThread.Abort();
                }
            }*/
        }
    }
}
