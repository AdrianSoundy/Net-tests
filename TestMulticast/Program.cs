using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Utils;

/// <summary>
/// Join a multiast group and read messages from it
/// </summary>
namespace TestMulticast
{
    public class Program
    {
        public static void Main()
        {
            // Setup and wait for Wireless ready
            Net.SetupAndConnectNetwork();

            Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            const int SOCK_DISCOVERY_MULTICAST_PORT = 26001;

            try
            {
                // Join Multicast Group
                // Multicast Address: 239.255.255.250
                Console.WriteLine("Join discovery group");
                byte[] multicastOpt = new byte[] { 239, 255, 255, 250, 0, 0, 0, 0 }; // IPAddress.Any: 0.0.0.0
                udpClient.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOpt);

                Console.WriteLine("Bind to discovery group port");
                udpClient.Bind(new IPEndPoint(IPAddress.Any, SOCK_DISCOVERY_MULTICAST_PORT ));

                while(true)
                {
                    byte[] rxbuffer = new byte[512];

                    udpClient.Receive(rxbuffer);

                    Console.WriteLine("Message from discovery group");
                }
            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
                Console.WriteLine("Exception :" + ex.Message);
                Console.WriteLine("stack     :" + ex.StackTrace);
            }
            finally
            {

                udpClient.Close();
            }

            while (true) Thread.Sleep(1000);
        }
    }
}
