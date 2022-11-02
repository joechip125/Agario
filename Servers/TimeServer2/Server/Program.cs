using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        private static TcpListener server = null;

        static void Main(string[] args)
        {
            
            Int32 port = 13000;
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            //DateTime.Now.ToLongTimeString();

            CancellationTokenSource cancelSource = new CancellationTokenSource();
            
            server = new TcpListener(localAddress, port);
            server.Start();
            
            while (true)
            {
                Console.WriteLine("Waiting for connection");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client Accepted");
                
                new Thread(() =>
                {
                    try
                    {
                        if (client.Connected)
                        {
                            var currentTime = DateTime.Now.ToLongTimeString();
                            var numBytes = Encoding.ASCII.GetByteCount(currentTime + 1);
                            byte[] dataToSend = new byte[numBytes];
                            dataToSend = Encoding.ASCII.GetBytes(currentTime);
                            
                            var culture = new CultureInfo("en-US");

                            using NetworkStream netStream = client.GetStream();
                        
                            var clientReader = new StreamReader(netStream);
                            var clientWriter = new  StreamWriter(netStream);
                            clientWriter.AutoFlush = true;
                            clientWriter.WriteLine("Make request");
                        
                            var input = clientReader.ReadLine();

                            switch (input)
                            {
                                case "time":
                                    clientWriter.WriteLine(DateTime.Now.ToString(culture));
                                    break;
                                case "time2":
                                    var message = EncodeMessage();
                                    
                                    netStream.Write(message, 0, message.Length);
                                    break;
                            }
                            
                            client.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }).Start();
            }
        }
        
        
        private static byte[] EncodeMessage()
        {
            var currentTime = DateTime.Now.ToLongTimeString();
            var numBytes = Encoding.UTF8.GetByteCount(currentTime + 1);
            byte[] dataToSend = new byte[numBytes];
            dataToSend = Encoding.UTF8.GetBytes(currentTime);
            return dataToSend;
        }
    }
    
}