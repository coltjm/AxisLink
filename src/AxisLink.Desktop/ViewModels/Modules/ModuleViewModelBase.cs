using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Avalonia.Controls;
using Dock.Model.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public abstract partial class ModuleViewModelBase : Document, IDisposable
    {
        protected ModuleViewModelBase()
        {
            Context = this;
        }
        public virtual void Dispose()
        {
        }
    }
}
