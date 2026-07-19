using Avalonia.Threading;
using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Tmds.DBus.Protocol;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class LoggerModuleViewModel : ModuleViewModelBase
    {
        private readonly IConsoleLogger _logger;
        // Maybe open up to user defined in future
        private const int MaxLogEntries = 1000;

        public ObservableCollection<KQLogEntry> UIEntries { get; } = new();

        public LoggerModuleViewModel(IConsoleLogger logger) 
        {
            _logger = logger;
            // When logger action is invoked, update UI
            _logger.OnLogReceived += OnLogAppend;
        }

        public LoggerModuleViewModel()
        {
            // Parameterless constructor for the Avalonia XAML Previewer / Designer
        }

        private void OnLogAppend(KQLogEntry logEntry)
        {
            // Update UI using Avalonia's UI thread dispatcher
            Dispatcher.UIThread.Post(() => AddLogEntry(logEntry));
        }

        private void AddLogEntry(KQLogEntry logEntry)
        {
            // Add new log entry to the UI collection
            UIEntries.Add(logEntry);
            // If we exceed max log entries, remove oldest entry
            if (UIEntries.Count > MaxLogEntries)
            {
                UIEntries.RemoveAt(0);
            }
        }
    }
}
