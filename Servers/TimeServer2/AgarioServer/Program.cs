using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using AgarioShared.AgarioShared.Enums;

namespace AgarioServer
{
    class Program
    {
        private static Int32 port = 13000;
        private static IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        private static TcpListener _server = null;
        private static Game _theGame = null;
        private static int playerCount;
        private static readonly int MaxPlayerCount = 
            Enum.GetValues(typeof(PlayerCounter)).Length;
        static void Main()
        {
            _server = new TcpListener(localAddress, port);
            _server.Start();
            int count = 0;
            while (true)
            {
                Console.WriteLine("Waiting for connection");
                TcpClient client = _server.AcceptTcpClient();
                Console.WriteLine("Client Accepted");

                if (_theGame == null)
                {
                    Console.WriteLine("Starting new game");
                    _theGame = new Game();
                    _theGame.GenerateRandomPositions(400);
                    _theGame.AddNewPlayer(client, (PlayerCounter) count);
                    new Thread(_theGame.Start).Start();
                    count++;
                }
                else if( _theGame.theLinks.Count < MaxPlayerCount)
                {
                    Console.WriteLine("Assigning Player to existing game.");
                    _theGame.AddNewPlayer(client, (PlayerCounter) count);
                    count++;
                }
                else
                {
                    Console.WriteLine("Maximum player count reached");
                }
                Console.WriteLine(count);
            }
        }

    }
}