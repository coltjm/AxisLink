using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Models.Logging
{

    public class KQLogEntry
    {
        public DateTime Timestamp { get; } = DateTime.Now;
        public LogLevel Severity { get; }
        public string Message { get; }

        public KQLogEntry(LogLevel severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        // Clean text fallback for text files or simple output engines
        public override string ToString() =>
            $"[{Timestamp:HH:mm:ss.fff}] {Severity.ToString().ToUpper()}: {Message}";
    }
}
