using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;

namespace AxisLink.Desktop.ViewModels.Windows.JogWindow
{
    public partial class JogWindowViewModel : ObservableObject
    {
        private readonly ShowFileManager _fileManager;
        private readonly MotionManager _motionManager;

        public Scenery? Scenery { get; }
        public IReadOnlyList<Axis> AttachedAxes { get; }

        [ObservableProperty] private float _jogVelocity = 30f;
        [ObservableProperty] private float _jogAcceleration = 5f;
        [ObservableProperty] private float _jogDeceleration = 5f;
        public string WindowTitle => $"Jog: {Scenery?.Name ?? "Unknown"}";

        public JogWindowViewModel(
            int sceneryId,
            ShowFileManager fileManager,
            MotionManager motionManager)
        {
            _fileManager = fileManager;
            _motionManager = motionManager;

            var show = _fileManager.CurrentShow;

            // 1. Resolve the Scenery Object from the Show
            Scenery = show?.Machinery.Scenery?.FirstOrDefault(s => s.Id == sceneryId);

            // 2. Resolve the linked Axis or Axes through the Patch
            AttachedAxes = ResolveAxes(sceneryId, show);
        }

        private IReadOnlyList<Axis> ResolveAxes(int sceneryId, ShowFile? show)
        {
            if (show?.Machinery.PatchList.Patches == null) return [];

            var patch = show.Machinery.PatchList.Patches.FirstOrDefault(p => p.Id == sceneryId);
            if (patch == null) return [];

            // 1:1 Axis Patch
            if (patch.AxisId.HasValue)
            {
                var axis = _motionManager.Axes.FirstOrDefault(a => a.Id == patch.AxisId.Value);
                return axis != null ? [axis] : [];
            }

            // Group Patch (Multi-motor sync / lift)
            if (patch.GroupId.HasValue)
            {
                var group = show.Machinery.Groups?.FirstOrDefault(g => g.Id == patch.GroupId.Value);
                if (group?.GroupAxes != null)
                {
                    return _motionManager.Axes
                        .Where(a => group.GroupAxes.Any(ga => ga.Id == a.Id))
                        .ToList();
                }
            }

            return [];
        }

        [RelayCommand]
        private void JogReverse()
        {
            foreach (var axis in AttachedAxes)
            {
                _motionManager.JogAxisAsync(axis.Id, velocity: JogVelocity, acceleration: JogAcceleration, deceleration: JogDeceleration);
            }
        }

        [RelayCommand]
        private void JogForward()
        {
            foreach (var axis in AttachedAxes)
            {
                _motionManager.JogAxisAsync(axis.Id, velocity: JogVelocity, acceleration: JogAcceleration, deceleration: JogDeceleration);
            }
        }

        [RelayCommand]
        private void Stop()
        {
            foreach (var axis in AttachedAxes)
            {
                //_motionManager.StopAxis(axis.Id);
            }
        }
    }
}