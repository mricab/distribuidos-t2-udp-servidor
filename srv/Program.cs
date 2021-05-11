using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace srv
{
    class Program
    {
        static void Main(string[] args)
        {
            // Greeting
            Console.WriteLine("Waiting for connections.");

            // Socket initilization
            UdpClient client = new UdpClient(8080);
            IPEndPoint remoteip = new IPEndPoint(IPAddress.Any,8080);

            // Stream
            byte[] receivedBytes = client.Receive(ref remoteip);
            if(receivedBytes != null) {
                string message = Encoding.ASCII.GetString(receivedBytes);
                Console.WriteLine("Message received: {0}", message);
            } else {
                Console.WriteLine("Empty Message Received.");
            }

            // Idle mode
            Console.ReadLine();
        }
    }
}
