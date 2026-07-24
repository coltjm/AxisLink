using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Models.Logging
{

    public class LogEntry
    {
        public DateTime Timestamp { get; } = DateTime.Now;
        public LogLevel Severity { get; }
        public string Message { get; }


        public LogEntry(LogLevel severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        // Clean text fallback for text files or simple output engines
        public override string ToString() =>
            $"[{Timestamp:HH:mm:ss.fff}] {Severity.ToString().ToUpper()}: {Message}";
    }
}
