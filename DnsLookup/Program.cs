using System;
using System.Threading;
using System.Net;
using Utils;

namespace DnsLookup
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                // Setup Wireless and wait to connect
                Net.SetupAndConnectNetwork();

                IPHostEntry hostEntry = Dns.GetHostEntry(Params.TEST_HOST_LOOKUP);
                if (hostEntry == null)
                    Console.WriteLine("Host name not found");
                else
                {
                    Console.WriteLine(" -- Host found --");
                    Console.WriteLine("Host name:" + hostEntry.HostName );
                    foreach (IPAddress ipa in hostEntry.AddressList)
                    {
                        Console.WriteLine("Host IP:" + ipa.ToString());
                    }

                }

                // Wait forever
                while(true)
                {
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
            }
        }
    }
}
