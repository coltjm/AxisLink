using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AxisLink.Desktop
{
    internal class ConsoleTesting
    {
        public static void Test(IServiceProvider serviceProvider)
        {
            var showFileManager = serviceProvider.GetRequiredService<ShowFileManager>();
            var motionManager = serviceProvider.GetRequiredService<MotionManager>();
            var logger = serviceProvider.GetRequiredService<IConsoleLogger>();
            logger.LogInfo("=== STARTING ARCHITECTURE TRACE ===");

            showFileManager.OpenShow("C:\\Users\\coltj\\source\\repos\\coltjm\\AxisLink\\src\\AxisLink.Tests\\testAxisLinkFile.txt");
            logger.LogInfo($"Successfully loaded show: {showFileManager.CurrentShow.Header.ShowName}");
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1000); // Simulate some delay before starting network connections
                    logger.LogInfo("[Background] Starting parallel network connections...");

                    await motionManager.StartupAsync();

                    logger.LogInfo("[Background] All network connections finalized successfully.");
                    throw new Exception("Simulated connection failure for testing purposes."); // Simulate a connection failure
                }
                catch (Exception ex)
                {
                    logger.LogError($"[Background ERROR] Connection failed: {ex.Message}");
                }
            });

            // 3. UI/Framework initialization finishes instantly here
            logger.LogInfo("=== MAIN THREAD INITIALIZATION COMPLETE ===");
        }
    }
}
