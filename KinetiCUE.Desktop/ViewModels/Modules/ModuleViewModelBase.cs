using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Desktop.ViewModels.Modules
{
    public abstract partial class ModuleViewModelBase : ViewModelBase
    {
        [ObservableProperty]
        private string _title = "New Module";

        public bool CanClose { get; set; } = true;
    }
}
