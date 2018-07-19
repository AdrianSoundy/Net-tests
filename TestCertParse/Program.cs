using System;
using System.Threading;
using System.Diagnostics;
using System.Text;

using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Utils;

namespace TestCertParse
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                // Wait for Wifi to connect
                Net.SetupAndConnectNetwork();

                for (int count = 0; count < 5; count++)
                {
                    ConnectSSLStream();
                }

                // Parse Certifciate using X509Certificate
                ParseCertificate();

 
            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
                Console.WriteLine("Main exception "+ex.Message);
            }
        }
        public static void ConnectSSLStream()
        {
            SslStream ssl;
            Socket ClientSocket;

            const string TargetIP = "192.168.2.126";
            const int TargetPort = 2322;

 //           const string TargetIP = "127.0.0.1";
            try
            {
                using (ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
                {
                    IPAddress ipa = IPAddress.Parse(TargetIP);
                    IPEndPoint ep = new IPEndPoint(ipa, TargetPort);
                    Console.WriteLine("Connecting to " + TargetIP);
                    ClientSocket.Connect(ep);

                    Console.WriteLine("Connected");

                    // Connected
                    using (ssl = new SslStream(ClientSocket))
                    {
                        Console.WriteLine("Setting up SSL");
                        // Do SSL handshake
                        ssl.AuthenticateAsClient(TargetIP, new SslProtocols[] { SslProtocols.SSLv3, SslProtocols.TLSv1 });

                        Console.WriteLine("SSL connected");

                        byte[] buffer = new byte[512];

                        int readLen = ssl.Read(buffer, 0, buffer.Length);
                        Console.WriteLine("Read "+readLen.ToString()+" bytes");
                    }

                    ClientSocket.Close();
                    Console.WriteLine("Connection closed");

                }


            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
                Console.WriteLine("ConnectSSLStream exception " + ex.Message);
            }
        }
            public static void ParseCertificate()

        {
            try
            {
                // User code goes here
                byte[] certBytes = GetCertificate();

                X509Certificate cert = new X509Certificate(certBytes);

                Console.WriteLine("Certificate Details:-");

                Console.WriteLine("Issuer:" + cert.Issuer);
                Console.WriteLine("Subject:" + cert.Subject);
                Console.WriteLine("EffectiveDate:" + cert.GetEffectiveDate().ToString());
                Console.WriteLine("EffectiveDate:" + cert.GetExpirationDate().ToString());

                if ( cert.GetRawCertData().Equals(GetCertificate()) )
                {
                    Console.WriteLine("Raw data ok");
                }
                else
                {
                    Console.WriteLine("Raw data different");
                } 

            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
            }
        }


        static byte[] GetCertificate()
        { 
            string scert =
                @"-----BEGIN CERTIFICATE-----" + "\n" +
                @"MIIEkjCCA3qgAwIBAgIQCgFBQgAAAVOFc2oLheynCDANBgkqhkiG9w0BAQsFADA/" + "\n" +
                @"MSQwIgYDVQQKExtEaWdpdGFsIFNpZ25hdHVyZSBUcnVzdCBDby4xFzAVBgNVBAMT" + "\n" +
                @"DkRTVCBSb290IENBIFgzMB4XDTE2MDMxNzE2NDA0NloXDTIxMDMxNzE2NDA0Nlow" + "\n" +
                @"SjELMAkGA1UEBhMCVVMxFjAUBgNVBAoTDUxldCdzIEVuY3J5cHQxIzAhBgNVBAMT" + "\n" +
                @"GkxldCdzIEVuY3J5cHQgQXV0aG9yaXR5IFgzMIIBIjANBgkqhkiG9w0BAQEFAAOC" + "\n" +
                @"AQ8AMIIBCgKCAQEAnNMM8FrlLke3cl03g7NoYzDq1zUmGSXhvb418XCSL7e4S0EF" + "\n" +
                @"q6meNQhY7LEqxGiHC6PjdeTm86dicbp5gWAf15Gan/PQeGdxyGkOlZHP/uaZ6WA8" + "\n" +
                @"SMx+yk13EiSdRxta67nsHjcAHJyse6cF6s5K671B5TaYucv9bTyWaN8jKkKQDIZ0" + "\n" +
                @"Z8h/pZq4UmEUEz9l6YKHy9v6Dlb2honzhT+Xhq+w3Brvaw2VFn3EK6BlspkENnWA" + "\n" +
                @"a6xK8xuQSXgvopZPKiAlKQTGdMDQMc2PMTiVFrqoM7hD8bEfwzB/onkxEz0tNvjj" + "\n" +
                @"/PIzark5McWvxI0NHWQWM6r6hCm21AvA2H3DkwIDAQABo4IBfTCCAXkwEgYDVR0T" + "\n" +
                @"AQH/BAgwBgEB/wIBADAOBgNVHQ8BAf8EBAMCAYYwfwYIKwYBBQUHAQEEczBxMDIG" + "\n" +
                @"CCsGAQUFBzABhiZodHRwOi8vaXNyZy50cnVzdGlkLm9jc3AuaWRlbnRydXN0LmNv" + "\n" +
                @"bTA7BggrBgEFBQcwAoYvaHR0cDovL2FwcHMuaWRlbnRydXN0LmNvbS9yb290cy9k" + "\n" +
                @"c3Ryb290Y2F4My5wN2MwHwYDVR0jBBgwFoAUxKexpHsscfrb4UuQdf/EFWCFiRAw" + "\n" +
                @"VAYDVR0gBE0wSzAIBgZngQwBAgEwPwYLKwYBBAGC3xMBAQEwMDAuBggrBgEFBQcC" + "\n" +
                @"ARYiaHR0cDovL2Nwcy5yb290LXgxLmxldHNlbmNyeXB0Lm9yZzA8BgNVHR8ENTAz" + "\n" +
                @"MDGgL6AthitodHRwOi8vY3JsLmlkZW50cnVzdC5jb20vRFNUUk9PVENBWDNDUkwu" + "\n" +
                @"Y3JsMB0GA1UdDgQWBBSoSmpjBH3duubRObemRWXv86jsoTANBgkqhkiG9w0BAQsF" + "\n" +
                @"AAOCAQEA3TPXEfNjWDjdGBX7CVW+dla5cEilaUcne8IkCJLxWh9KEik3JHRRHGJo" + "\n" +
                @"uM2VcGfl96S8TihRzZvoroed6ti6WqEBmtzw3Wodatg+VyOeph4EYpr/1wXKtx8/" + "\n" +
                @"wApIvJSwtmVi4MFU5aMqrSDE6ea73Mj2tcMyo5jMd6jmeWUHK8so/joWUoHOUgwu" + "\n" +
                @"X4Po1QYz+3dszkDqMp4fklxBwXRsW10KXzPMTZ+sOPAveyxindmjkW8lGy+QsRlG" + "\n" +
                @"PfZ+G6Z6h7mjem0Y+iWlkYcV4PIWL1iwBi8saCbGS5jN2p8M+X+Q7UNKEkROb3N6" + "\n" +
                @"KOqkqm57TH2H3eDJAkSnh6/DNFu0Qg==" + "\n" +
                @"-----END CERTIFICATE-----" + "\n" +
                "X";  // place marker for terminator

            byte[] ret = UTF8Encoding.UTF8.GetBytes(scert);
            ret[ret.Length-1] = 0;

            return ret;
        }
    }
}
