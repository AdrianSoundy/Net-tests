using System;
using System.Threading;

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.Security;
using Utils;

namespace TestSecureClient
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                Console.WriteLine("Program started");

                Console.WriteLine("Setup Wifi/network and wait till online");
                Net.SetupAndConnectNetwork();
                Console.WriteLine("Wifi connected");

                while (true)
                {
                    try
                    {

                        Console.WriteLine("Opening socket");
                        using (Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
                        {
                            try
                            {
                                IPAddress ipa = IPAddress.Parse("192.168.2.129");
                                IPEndPoint ep = new IPEndPoint(ipa, 2322);

                                soc.Connect(ep);
                                
                                Console.WriteLine("connected");
                                SslStream ss = new SslStream(soc);
                                ss.AuthenticateAsClient("adrian-laptop2", SslProtocols.TLSv1); //| SslProtocols.TLSv11| SslProtocols.TLSv12);

                                Console.WriteLine("SSL handshake ok");
                                Console.WriteLine("SSL isServer:"+ ss.IsServer.ToString());

                                while (true)
                                {
                                    byte[] buffer = new byte[1024];

                                    int bytes = ss.Read(buffer, 0, buffer.Length);

                                    Console.WriteLine("Read bytes" + bytes.ToString());

                                }
                            }
                            catch(Exception)
                            {
                                soc.Close();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception "+ex.Message);
                    }
                    Console.WriteLine("wait connect");
                    Thread.Sleep(5000);
                } // while


            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
            }
        }

     }
}
