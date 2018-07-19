using System;
using System.Threading;
using System.Diagnostics;

using System.Net;
using System.Net.NetworkInformation;
using Utils;

namespace TestWirelessConnectAP
{
    public class Program
    {
        public static void Main()
        {
            string SSID = Params.SSID;
            string PASSWORD = Params.AP_PASSWORD;

            try
            {
                Console.WriteLine("Wireless connect AP test - availability");

                NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
                
                // Get firt interface for ESP32 ( Wireless AP )
                NetworkInterface wi = NetworkInterface.GetAllNetworkInterfaces()[0];
                wi.EnableDhcp();

                // Get ID of wireless config ( should be 0 )
                uint wirelessConfigId = wi.SpecificConfigId;

                Wireless80211Configuration wirelessApConfig = Wireless80211Configuration.GetAllWireless80211Configurations()[wirelessConfigId];
                
                // Set up the Access Point detail we want to connect to 
                wirelessApConfig.Ssid = SSID;
                wirelessApConfig.Password = PASSWORD;
                wirelessApConfig.Authentication = Wireless80211Configuration.AuthenticationType.WPA2;
                
                // Save config so it will be persisted over a reboot
                
                wirelessApConfig.SaveConfiguration();
 
                // Currenlty need to reboot if this is first time run
                while(true)
                {

                    NetworkInterface wi2 = NetworkInterface.GetAllNetworkInterfaces()[0];
                    if (wi2.IPv4Address[0] != 0) break;
                    Console.WriteLine("No IP yet, not connected");
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Have IP so must be connected");

                while (true)
                {
                    Thread.Sleep(1000);
                }
                    // User code goes here
            }
            catch (Exception ex)
            {
                // Do whatever please you with the exception caught
            }
        }

        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Console.WriteLine(e.IsAvailable ? "Network is available" : "Network is not available");
        }

        private static void NetworkChange_NetworkAddressChanged(object sender, nanoFramework.Runtime.Events.EventArgs e)
        {
            Console.WriteLine( "Network address changed");

        }
    }
}
