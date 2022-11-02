using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UDPServer
{
    class Program
    {
        private static UdpClient server = new UdpClient(1500);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11000);
        
        private static EndPoint _endPoint;
        private static string compoundString;
      
   

        static void Main(string[] args)
        {
            new Thread(() =>
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
                UdpClient newsock = new UdpClient(ipep);
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                
                while (true)
                {
                    Console.WriteLine("Waiting for message");
                    var data = newsock.Receive(ref sender);
                    var theString = ConvertBytesToString(data);
                    
                    if (theString != "ERROR")
                    {
                        compoundString += theString + " ";
                        Console.WriteLine(compoundString);
                        var byteArray = ConvertStringToByteArray(compoundString);
    
                        server.Send(byteArray, byteArray.Length, sender);
                        Console.WriteLine("Message sent");
                    }
                    else
                    {
                        Console.WriteLine("Error");
                    }
                }
            }).Start();
        }


        private static byte[] ConvertStringToByteArray(string theString)
        {
            var count = Encoding.UTF8.GetByteCount(theString);
            var byteArray = new byte[count];
            byteArray = Encoding.UTF8.GetBytes(theString);
            
            return byteArray;
        }
        
        
        private static string ConvertBytesToString(byte[] bytes)
        {
            var numBytes = bytes.Count(x => x != 0);
            var charArray = Encoding.UTF8.GetString(bytes, 0, numBytes);
            var charCount = 0;
            var outString = "";

            foreach (var c in charArray)
            {
                if (Char.IsWhiteSpace(c) || charCount > 20)
                {
                    outString = "ERROR";
                    break;
                }
                else
                {
                    outString += c;
                }
                
                charCount++;
            }

            return outString;
        }
    }
}