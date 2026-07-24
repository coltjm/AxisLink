using AxisLink.Core.Models.Show;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AxisLink.Desktop.ViewModels.Windows.CueCreation
{
    public partial class CueCreationWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Cue _cue;
        [ObservableProperty]
        private CuePart _selectedCuePart;

        public ObservableCollection<CuePart> CueParts { get; } = new();

        public string WindowTitle => string.IsNullOrWhiteSpace(Cue.Number)
            ? "Create New Cue"
            : $"Edit Cue {Cue.Number}";

        public CuePartMoveType[] AvailableMoveTypes => Enum.GetValues<CuePartMoveType>();
        public CuePartStartType[] AvailableStartTypes => Enum.GetValues<CuePartStartType>();
        public CuePartTargetType[] AvailableTargetTypes => Enum.GetValues<CuePartTargetType>();

        // Dynamic Visibility Properties for Inspector
        public bool IsStartAbsolute => SelectedCuePart?.Start?.Type == CuePartStartType.Absolute;
        public bool IsStartLimit => SelectedCuePart?.Start?.Type == CuePartStartType.Limit;
        public bool IsStartTrim => SelectedCuePart?.Start?.Type == CuePartStartType.Trim;

        public bool IsTargetAbsolute => SelectedCuePart?.Target?.Type == CuePartTargetType.Absolute;
        public bool IsTargetRelative => SelectedCuePart?.Target?.Type == CuePartTargetType.Relative;
        public bool IsTargetLimit => SelectedCuePart?.Target?.Type == CuePartLimitType();

        // Helper proxies to manage nested nullability cleanly in Avalonia bindings
        public CuePartStartType? SelectedStartType
        {
            get => SelectedCuePart?.Start?.Type;
            set
            {
                if (SelectedCuePart == null) return;
                SelectedCuePart.Start ??= new CuePartStart();
                SelectedCuePart.Start.Type = value;
            }
        }

        public CuePartTargetType? SelectedTargetType
        {
            get => SelectedCuePart?.Target?.Type;
            set
            {
                if (SelectedCuePart == null) return;
                SelectedCuePart.Target ??= new CuePartTarget { Speed = 0 };
                SelectedCuePart.Target.Type = value;
            }
        }

        public CueCreationWindowViewModel(Cue? existingCue = null)
        {
            _cue = existingCue ?? new Cue { Number = "1.0", Name = "New Cue" };
            CueParts = new ObservableCollection<CuePart>(Cue.CueParts);

        }

        [RelayCommand]
        private void AddCuePart()
        {
            var newPart = new CuePart
            {
                Id = CueParts.Count + 1,
                MoveType = CuePartMoveType.Linear,
                Start = new CuePartStart { Type = CuePartStartType.Absolute, Position = 0 },
                Target = new CuePartTarget { Type = CuePartTargetType.Absolute, Position = 0, Speed = 100 }
            };
            CueParts.Add(newPart);
            SelectedCuePart = newPart;
        }

        [RelayCommand]
        private void RemoveCuePart()
        {
            if (SelectedCuePart != null)
            {
                CueParts.Remove(SelectedCuePart);
                SelectedCuePart = CueParts.FirstOrDefault();
            }
        }

        private CuePartTargetType CuePartLimitType() => CuePartTargetType.Limit;

        [RelayCommand]
        private void Save()
        {
            //Cue.CueParts = CueParts.ToList();
            // TODO: Signal back to Window/DialogResult or EventBus
        }

        [RelayCommand]
        private void Cancel()
        {
            // Close window or discard changes
        }
    }
}
