using System.Collections.ObjectModel;
using System.Linq;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Desktop.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class SceneryCardViewModel : ObservableObject
    {
        private readonly MotionManager _motionManager;
        private readonly ShowFileManager _fileManager;
        private readonly WindowManager _windowManager;

        public Scenery Scenery { get; }
        public ObservableCollection<Axis> AttachedAxes { get; } = new();

        public SceneryCardViewModel(
            Scenery scenery,
            MotionManager motionManager,
            ShowFileManager fileManager,
            WindowManager windowManager)
        {
            Scenery = scenery;
            _motionManager = motionManager;
            _fileManager = fileManager;
            _windowManager = windowManager;

            ResolveAttachedAxes();
        }

        private void ResolveAttachedAxes()
        {
            AttachedAxes.Clear();
            var currentShow = _fileManager.CurrentShow;
            if (currentShow?.Machinery.PatchList.Patches == null) return;

            // Find patch entry linking this Scenery to Axis or Group
            var patch = currentShow.Machinery.PatchList.Patches.FirstOrDefault(p => p.Id == Scenery.Id);
            if (patch == null) return;

            if (patch.AxisId.HasValue)
            {
                var axis = _motionManager.Axes.FirstOrDefault(a => a.Id == patch.AxisId.Value);
                if (axis != null) AttachedAxes.Add(axis);
            }
            else if (patch.GroupId.HasValue)
            {
                var group = currentShow.Machinery.Groups.FirstOrDefault(g => g.Id == patch.GroupId.Value);
                if (group?.GroupAxes != null)
                {
                    foreach (var member in group.GroupAxes)
                    {
                        var axis = _motionManager.Axes.FirstOrDefault(a => a.Id == member.Id);
                        if (axis != null) AttachedAxes.Add(axis);
                    }
                }
            }
        }

        [RelayCommand]
        private void OpenJog()
        {
            _windowManager.ShowJogWindow(Scenery.Id);
        }
    }
}