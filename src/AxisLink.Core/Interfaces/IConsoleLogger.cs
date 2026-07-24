using AxisLink.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    // Allow for 4 different levels of logging
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }

    // IConsoleLogger to avoid conflict with Serilog's ILogger
    public interface IConsoleLogger
    {
        // Each log has a level, message, and optional exception in case of errors/warnings
        void Log(LogLevel level, string message, Exception? ex = null);

        // Event to notify when a log entry is received
        event Action<LogEntry>? OnLogReceived;

        // Convenience methods for each log level
        void LogError(string message, Exception? ex = null);
        void LogWarning(string message, Exception? ex = null);
        void LogInfo(string message, Exception? ex = null);
        void LogCritical(string message, Exception? ex = null);
    }
}
