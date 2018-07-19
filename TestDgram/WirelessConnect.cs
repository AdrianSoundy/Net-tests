using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace Utils
{
	public class Net
    {
        /// <summary>
        /// Initialise the Wireless parameters and save
        /// </summary>
        public static void SetupAndConnectWifi()
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            if (nis.Length > 0)
            {
                NetworkInterface ni = nis[0];       // get Wifi interface
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    Wireless80211Configuration wc = Wireless80211Configuration.GetAllWireless80211Configurations()[ni.SpecificConfigId];
                    if (wc.Ssid != Params.SSID && wc.Password != Params.AP_PASSWORD)
                    {
                        Console.WriteLine("Updating Wifi config");
                        wc.Ssid = Params.SSID;
                        wc.Password = Params.AP_PASSWORD;
                        wc.SaveConfiguration();

                        ni.EnableDhcp();
                    }
                    else
                    {
                        Console.WriteLine("Wifi config ok");
                    }

                    Console.WriteLine("Wait for IP");
                    while (true)
                    {
                        NetworkInterface ni2 = NetworkInterface.GetAllNetworkInterfaces()[0];
                        if (ni2.IPv4Address != null && ni2.IPv4Address.Length > 0)
                        {
                            if (ni2.IPv4Address[0] != '0')
                            {
                                Console.WriteLine("Have IP " + ni2.IPv4Address);
                                break;
                            }
                        }

                        Thread.Sleep(1000);
                    }


                }
                else
                    throw new NotSupportedException("No Wifi");

            }
            else
                throw new NotSupportedException("No Network");

        }




    }



}
