using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using Windows.Devices.Gpio;
using Utils;

namespace TestConnect
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Program started");
            try
            {

                Console.WriteLine("Setup Wifi/network and wait till online ");
                Net.SetupAndConnectNetwork();
                Console.WriteLine("Wifi/network connected");

                GpioPin pin = GpioController.GetDefault().OpenPin(27);
                //GpioPin pin = GpioController.GetDefault().OpenPin(PinNumber('J', 5));
                pin.SetDriveMode(GpioPinDriveMode.Output);
                pin.Write(GpioPinValue.High);

                for (; ; )
                {
                    ListenTest(pin);

                    Thread.Sleep(2000);
                }
            }
            catch( Exception ex)
            {
                Console.WriteLine("exception " + ex.Message);
                Console.WriteLine("exception " + ex.StackTrace);

            }

            while(true)
            {
                Thread.Sleep(100);
            }
        }

        static void PinPulse(GpioPin pin, int timeMs)
        {
            pin.Write(GpioPinValue.Low);
            Thread.Sleep(timeMs);
            pin.Write(GpioPinValue.High);
        }

        static void ListenTest(GpioPin pin)
        {
            PinPulse(pin, 2000);

            using (Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, 6501);

                soc.Bind(ep);

                soc.Listen(1);

                while (true)
                {
                    Socket client = null;

                    int BytesRead;
                    try
                    {
                        Console.WriteLine( DateTime.UtcNow.ToString() + "Accept client");
                        client = soc.Accept();
                        Console.WriteLine(DateTime.UtcNow.ToString() + "Accepted");

                        PinPulse(pin, 500);
                            
                        client.ReceiveTimeout = 10000;

                        NetworkStream stream = new NetworkStream(client);

                        byte[] buffer = new byte[100];

                        // Echo back everything we receive and toggle Led 
                        while ((BytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, BytesRead);
                            Console.WriteLine(DateTime.UtcNow.ToString() + "Read/Write " + BytesRead.ToString());
                            pin.Toggle();
                        }

                        Console.WriteLine(DateTime.UtcNow.ToString() + "connection ending");
                    }
                    catch (Exception ex) {
                        Console.WriteLine(DateTime.UtcNow.ToString() + "Ex " + ex.Message);

                    };

                    if (client != null ) client.Close();
                    break;
                }
            }
        }


        //    static void ConnectTest(GpioPin pin)
        //    {

        //        while (true)
        //        {
        //            try
        //            {
        //                using (Socket soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IPv4))
        //                {
        //                    IPAddress ipa = IPAddress.Parse("192.168.2.129");

        //                    IPEndPoint ep = new IPEndPoint(ipa, 6500);

        //                    pin.Write(GpioPinValue.Low);

        //                    Type sType = Type.GetType("System.Net.Sockets.Socket");
        //                    FieldInfo blockingInfo = sType.GetField("m_fBlocking", BindingFlags.NonPublic | BindingFlags.Instance);
        //                    blockingInfo.SetValue(soc, false);

        //                    soc.Connect(ep);

        //                    pin.Write(GpioPinValue.High);
        //                    Console.WriteLine("Connected");

        //                    byte[] buf = new byte[] { 0x41, 0x42, 0x43 };
        //                    soc.Send(buf);
        //                    Console.WriteLine("send");

        //                    soc.ReceiveTimeout = 1000;
        //                    byte[] bufr = new byte[3];
        //                    soc.Receive(bufr);
        //                    Console.WriteLine("receive");

        //                    soc.Close();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error" + ex.Message);
        //            }
        //            finally
        //            {
        //                Thread.Sleep(2000);
        //            }
        //        }
        //        }

        static int PinNumber(char port, byte pin)
        {
            if (port < 'A' || port > 'J')
                throw new ArgumentException();

            return ((port - 'A') * 16) + pin;
        }
    }
}
