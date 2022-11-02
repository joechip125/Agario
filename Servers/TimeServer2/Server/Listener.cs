using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Listener
    {
        private TcpListener server;
        Byte[] bytes = new Byte[256];
        String data = null;

        public async void Init()
        {
            Int32 port = 13000;
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");

            CancellationTokenSource cancelSource = new CancellationTokenSource();
            

            server = new TcpListener(localAddress, port);
            server.Start();
            while (true)
            {
                Console.WriteLine("Waiting for connection");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client Accepted");
                
            }

            await Task.Run(() =>
            {
                while (!cancelSource.IsCancellationRequested)
                {
                    var tcpClient = server.AcceptTcpClientAsync();
                }
            });
        }

        public void Run()
        {
            while (true)
            {
                
            }
        }
        
    }
}