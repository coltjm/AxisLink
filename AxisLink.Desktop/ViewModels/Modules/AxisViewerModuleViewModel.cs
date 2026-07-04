using AxisLink.Core.Management;
using AxisLink.Core.Models.Extended;
using AxisLink.Core.Models.Standard;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class AxisViewerModuleViewModel : ModuleViewModelBase
    {
        private readonly ShowFileManager FileManager;
        [ObservableProperty]
        List<ExtendedAxis> axes;
        List<StandardAxis> standardAxes;
        public AxisViewerModuleViewModel(ShowFileManager fileManager)
        {
            FileManager = fileManager;
            Title = "Motor Axis Viewer";
            // All axes are stored as standard axes in the show file
            standardAxes = FileManager.CurrentShow?.Machinery?.Axes ?? new List<StandardAxis>();
            axes = new List<ExtendedAxis>();
            foreach (StandardAxis axis in standardAxes) {
                if(axis is  ExtendedAxis extendedAxis) {
                    axes.Add(extendedAxis);
                }
                else
                {
                    //log error
                }
            }
        }


    }
}
