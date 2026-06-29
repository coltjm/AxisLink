using KinetiCUE.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Interfaces
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Critical
    }

    // KQLogger to avoid conflict with Serilog's ILogger
    public interface IKQLogger
    {
        void Log(LogLevel level, string message, Exception? ex = null);
        event Action<KQLogEntry>? OnLogReceived;
        void LogError(string message, Exception? ex = null);
        void LogWarning(string message, Exception? ex = null);
        void LogInfo(string message, Exception? ex = null);
        void LogCritical(string message, Exception? ex = null);
    }
}
