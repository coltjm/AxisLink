using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public abstract partial class ModuleViewModelBase : ViewModelBase, IDisposable
    {
        [ObservableProperty]
        private string _title = "New Module";

        public bool CanClose { get; set; } = true;

        public virtual void Dispose()
        {
            
        }
    }
}
