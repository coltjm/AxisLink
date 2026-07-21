using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace AxisLink.Desktop
{
    internal class ConsoleTesting
    {
        public static void Test(IServiceProvider serviceProvider)
        {
            var showFileManager = serviceProvider.GetRequiredService<ShowFileManager>();
            //var motionManager = serviceProvider.GetRequiredService<MotionManager>();
            var logger = serviceProvider.GetRequiredService<IConsoleLogger>();
            logger.LogInfo("=== STARTING CONSOLE TESTING ===");

            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000); // Simulate some delay before starting network connections
                    logger.LogInfo("[Background] Starting parallel network connections...");

                    //await motionManager.StartupAsync();

                    logger.LogInfo("[Background] All network connections finalized successfully.");
                    throw new Exception("Simulated connection failure for testing purposes."); // Simulate a connection failure

                }
                catch (Exception ex)
                {
                    logger.LogError($"[Background ERROR] Connection failed: {ex.Message}");
                }
                ShowNetworkInterfaces(logger);
            });

            
            logger.LogInfo("=== MAIN THREAD INITIALIZATION COMPLETE ===");
        }
        // TESTING METHOD TO SHOW NETWORK INTERFACES
        public static void ShowNetworkInterfaces(IConsoleLogger logger)
        {
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            logger.LogInfo("Interface information for " + computerProperties.HostName + "." + computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                logger.LogInfo("  No network interfaces found.");
                return;
            }

            logger.LogInfo("  Number of interfaces .................... : " + nics.Length);
            foreach (NetworkInterface adapter in nics)
            {
                if(adapter.GetPhysicalAddress().ToString() != "" && adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet && adapter.GetIPProperties().UnicastAddresses.Count > 0)
                { 
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    logger.LogInfo(adapter.Description);
                    logger.LogInfo(String.Empty.PadLeft(adapter.Description.Length, '='));
                    logger.LogInfo("  Interface type .......................... : " + adapter.NetworkInterfaceType);
                    logger.LogInfo("  Physical Address ........................ : " + adapter.GetPhysicalAddress().ToString());
                    logger.LogInfo("  Operational status ...................... : " + adapter.OperationalStatus);
                    string versions = "";

                    // Create a display string for the supported IP versions.
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        versions = "IPv4";
                    }
                    if (adapter.Supports(NetworkInterfaceComponent.IPv6))
                    {
                        if (versions.Length > 0)
                        {
                            versions += " ";
                        }
                        versions += "IPv6";
                    }
                    logger.LogInfo("  IP version .............................. : " + versions);
                    //logger.LogInfo("  IP Address ............................... : "+ properties.UnicastAddresses.ToString());
                    logger.LogInfo("  Name.................................. : " + adapter.Name);
                    //ShowIPAddresses(properties);
                    UnicastIPAddressInformationCollection unicast = properties.UnicastAddresses;
                    if (unicast.Count > 0)
                    {
                        logger.LogInfo("  Unicast Addresses ....................... :");
                        foreach (UnicastIPAddressInformation ip in unicast)
                        {
                            logger.LogInfo("    " + ip.Address.ToString());
                            logger.LogInfo("    Subnet Mask ............................ : " + ip.IPv4Mask.ToString());
                            
                        }
                    }
                    // The following information is not useful for loopback adapters.
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    {
                        continue;
                    }
                    logger.LogInfo("  DNS suffix .............................. : " + properties.DnsSuffix);

                    string label;
                    if (adapter.Supports(NetworkInterfaceComponent.IPv4))
                    {
                        IPv4InterfaceProperties ipv4 = properties.GetIPv4Properties();
                        logger.LogInfo("  MTU...................................... : " + ipv4.Mtu);
                        if (ipv4.UsesWins)
                        {

                            IPAddressCollection winsServers = properties.WinsServersAddresses;
                            if (winsServers.Count > 0)
                            {
                                label = "  WINS Servers ............................ :";
                                //ShowIPAddresses(label, winsServers);
                            }
                        }
                    }

                    logger.LogInfo("  DNS enabled ............................. : " + properties.IsDnsEnabled);
                    logger.LogInfo("  Dynamically configured DNS .............. : " + properties.IsDynamicDnsEnabled);
                    logger.LogInfo("  Receive Only ............................ : " + adapter.IsReceiveOnly);
                    logger.LogInfo("  Multicast ............................... : " + adapter.SupportsMulticast);
                    //ShowInterfaceStatistics(adapter);
                }
            }
        }
    }
}
