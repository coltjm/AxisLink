using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class CueStackModuleViewModel : ModuleViewModelBase
    {
        private readonly IConsoleLogger Logger;
        private readonly CueManager cueManager;
        public CueStackModuleViewModel(IConsoleLogger logger, CueManager _cueManager)
        {
            Logger = logger;
            cueManager = _cueManager;
            Title = "Cue Stack Viewer";
        }

    }
}
