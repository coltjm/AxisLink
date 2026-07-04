using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AxisLink.Infrastructure.Loggers
{
    public class KQLogger : IConsoleLogger
    {
        private readonly ILogger _logger;
        // Buffer to hold log entries until the UI is ready
        private readonly List<KQLogEntry> _historyBuffer = new();
        // Can open up to user defined in future
        private const int MaxBufferCount = 500;
        // Create lock object to ensure only one thread accesses event at a time
        private readonly object _lock = new();
        
        // Private backing field for the event to allow for custom add/remove logic
        private Action<KQLogEntry>? _onLogReceived;
        // Public event to allow external subscribers to register for log entry notifications
        public event Action<KQLogEntry>? OnLogReceived
        {
            add
            {
                // inside of add and remove, value is equal to the callback method being added or removed from the event handler chain

                // Locks action and history buffer
                lock (_lock)
                {
                    // Register callback method to event handler chain
                    _onLogReceived += value;

                    // Execute the callback for each log entry in the history buffer
                    foreach (var line in _historyBuffer)
                    {
                        value(line);
                    }
                }
            }
            remove
            {
                // Locks action while modifying chain
                lock (_lock)
                {
                    _onLogReceived -= value;
                }
            }
        }

        public KQLogger()
        {
            // Set up Serilog to log to a file in the user's AppData directory (or other location as needed)
            string logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AxisLink", "logs");
            Directory.CreateDirectory(logDir);
            string logPath = Path.Combine(logDir, "AxisLink.log");

            // Create a Serilog logger
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logPath, 
                rollingInterval: RollingInterval.Day, 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        public void Log(LogLevel level, string message, Exception? ex = null)
        {
            // Copy logs to debug output for development purposes
            Debug.WriteLine(message);
            // Include exception message in the log entry if an exception is provided
            string cleanMessage = ex != null ? $"{message} | Ex: {ex.Message}" : message;
            // Create log entry in the KQLogEntry format
            var entry = new KQLogEntry(level, cleanMessage);
            // Lock buffer while reading and writing
            lock (_lock)
            {
                
                _historyBuffer.Add(entry);
                if (_historyBuffer.Count > MaxBufferCount)
                {
                    _historyBuffer.RemoveAt(0);
                }
                
                _onLogReceived?.Invoke(entry);
                
            }

            // Log to Serilog based on the log level
            switch (level)
            {
                case LogLevel.Info: _logger.Information(ex, message); break;
                case LogLevel.Warning: _logger.Warning(ex, message); break;
                case LogLevel.Error: _logger.Error(ex, message); break;
                case LogLevel.Critical: _logger.Fatal(ex, message); break;
            }

        }

        // Helper methods for logging at specific levels
        public void LogError(string message, Exception? ex = null) => Log(LogLevel.Error, message, ex);
        public void LogWarning(string message, Exception? ex = null) => Log(LogLevel.Warning, message, ex);
        public void LogInfo(string message, Exception? ex = null) => Log(LogLevel.Info, message, ex);
        public void LogCritical(string message, Exception? ex = null) => Log(LogLevel.Critical, message, ex);
    }
}
