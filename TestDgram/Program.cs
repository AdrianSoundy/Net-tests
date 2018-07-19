using System;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using Utils;

/// <summary>
/// Test the Datagram functionality
/// 
/// Send datagram to specified target
/// Send datagram to sub net
/// Receive datagram
/// </summary>

namespace TestDgram
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                byte[] wbuffer = new byte[128];
                byte[] rbuffer = new byte[128];

                Net.SetupAndConnectNetwork();

                Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress ipa = IPAddress.Parse(Params.TEST_TARGET_IP);

                IPEndPoint ep = new IPEndPoint(ipa, Params.TEST_TARGET_PORT);

                soc.ReceiveTimeout = 3000;

                // Send message to specific system and expect echo
                for (int count = 0; count < 10; count++)
                {
                    // Send 128 bytes
                    wbuffer[0] = (byte)count;
                    soc.SendTo(wbuffer, ep);

                    Console.WriteLine("Dgram sent");

                    // Expect 128 back with correct first byte
                    EndPoint rep = new IPEndPoint(IPAddress.Any, 0);
                    int rxcount = soc.ReceiveFrom(rbuffer, ref rep);
                    Console.WriteLine("Dgram rx:" + rxcount.ToString());

                    // Check reply
                    if ( rxcount == wbuffer.Length && 
                         ep.Equals((IPEndPoint)rep) &&
                         rbuffer[0] == count ) 
                    {
                        // Ok reply
                        continue;
                    }

                    Console.WriteLine("Dgram echo failed");
                    break;
                }

                Console.WriteLine("Send message to subnet");
                soc.Close();

                soc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                soc.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

                // Create a subnet address
                byte[] adrSubnet = ipa.GetAddressBytes();
                adrSubnet[3] = 255;

                IPAddress ipas = new IPAddress(adrSubnet);
                IPEndPoint sep = new IPEndPoint(ipas, Params.TEST_TARGET_PORT);

                
                for (int count = 0; count < 10; count++)
                {
                    // Send 128 bytes
                    wbuffer[0] = (byte)count;
                    soc.SendTo(wbuffer, sep);
                    Console.WriteLine("Dgram broadcast sent");

                    // Expect 128 back with correct first byte
                    EndPoint rep = new IPEndPoint(IPAddress.Any, 0);
                    int rxcount = soc.ReceiveFrom(rbuffer, ref rep);
                    Console.WriteLine("Dgram rx:"+rxcount.ToString());

                    // Check reply
                    if (rxcount == wbuffer.Length &&
                         ep.Equals((IPEndPoint)rep) &&
                         rbuffer[0] == count)
                    {
                        // Ok reply
                        continue;
                    }

                    Console.WriteLine("Subnet Dgram echo failed");
                    break;



                }


                }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:"+ex.Message);
                Console.WriteLine("Stack:" + ex.StackTrace);
                // Do whatever please you with the exception caught
            }

            Console.WriteLine("Test end");
            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
