using System;
using System.Net;
using System.Net.Sockets;

namespace NetTester
{
    /// <summary>
    /// Desktop console app used or testing Nanoframework network UDP messages
    /// Just echo back data
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient uc = new UdpClient(62345, AddressFamily.InterNetwork);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                
            while (true)
            {
                byte[] rbytes = uc.Receive(ref remoteEP);
                uc.Send(rbytes, rbytes.Length, remoteEP);
            }

        }
    }
}
