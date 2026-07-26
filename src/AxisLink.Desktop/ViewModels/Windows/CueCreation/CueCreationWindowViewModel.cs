using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
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
        private readonly MotionManager motionManager;
        private readonly CueManager cueManager;
        public ObservableCollection<Axis> AvailableAxes { get; }
        [ObservableProperty]
        private Cue _cue;
        [ObservableProperty]
        private CuePart _selectedCuePart;
        [ObservableProperty]
        private Axis _selectedAxis;
        [ObservableProperty]
        private KeyValuePair<CuePartMoveType, string> _selectedMoveType;
        [ObservableProperty]
        private Dictionary<CuePartMoveType, string> _moveTypeOptions;

        // Map enum to string
        private readonly Dictionary<CuePartMoveType, string> moveTypeNames = new Dictionary<CuePartMoveType, string>
        {
            { CuePartMoveType.Linear, "Linear" },
            { CuePartMoveType.RotaryCW, "Rotary Clockwise" },
            { CuePartMoveType.RotaryCCW, "Rotary Counter Clockwise" },
            { CuePartMoveType.RotaryShortest, "Rotary Shortest" },
            { CuePartMoveType.ContinuousIncreasing, "Continuous Increasing" },
            { CuePartMoveType.ContinuousDecreasing, "Continuous Decreasing" },
            { CuePartMoveType.Joystick, "Joystick" }
        };

        public ObservableCollection<CuePart> CueParts { get; } = new();
        [ObservableProperty]
        private HierarchicalTreeDataGridSource<CuePart> _cuePartsSource;
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
        public bool IsTargetLimit => SelectedCuePart?.Target?.Type == CuePartTargetType.Limit;

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

        public CueCreationWindowViewModel(CueManager _cueManager, MotionManager _motionManager, Cue? existingCue = null)
        {
            _cue = existingCue ?? new Cue { Number = "1.0", Name = "New Cue" };
            CueParts = new ObservableCollection<CuePart>(Cue.CueParts);
            _cuePartsSource = new HierarchicalTreeDataGridSource<CuePart>(CueParts)
                .WithHierarchicalExpanderTextColumn("Delay", x => x.Target.Delay, x => x.Children)
                .WithTextColumn("Scenery ID", x => x.Id)
                .WithTextColumn("Target", x => x.Target.Position)
                .WithTextColumn("Velocity", x => x.Target.Speed)
                .WithTextColumn("Acceleration", x => x.Target.Accel)
                .WithTextColumn("Deceleration", x => x.Target.Decel);
            cueManager = _cueManager;
            motionManager = _motionManager;
            MoveTypeOptions = moveTypeNames;
            AvailableAxes = new ObservableCollection<Axis>(_motionManager.Axes);
        }

        [RelayCommand]
        private void AddCuePart()
        {
            var newPart = new CuePart
            {
                Id = CueParts.Count + 1,
                MoveType = CuePartMoveType.Linear,
                Start = new CuePartStart { Type = CuePartStartType.Absolute, Position = 0 },
                Target = new CuePartTarget { Type = CuePartTargetType.Absolute, Position = 0, Speed = 100, Delay = 0 },
               
                
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

        partial void OnSelectedAxisChanged(Axis value)
        {
            MoveTypeOptions.Clear();
            MoveTypeOptions = moveTypeNames;
            // set the options for move type
            if (value.Type == AxisType.Other)
            {
                // keep

            }
            else if (value.Type == AxisType.Rotary)
            {
                // only allow rotation and continuous
                MoveTypeOptions.Remove(CuePartMoveType.Linear);

            }
            else
            {
                // Some type of linear
                MoveTypeOptions.Remove(CuePartMoveType.RotaryCW);
                MoveTypeOptions.Remove(CuePartMoveType.RotaryCCW);
                MoveTypeOptions.Remove(CuePartMoveType.RotaryShortest);
            }
            // If the selected move type is still valid, keep it
            // else change to default
            if(!MoveTypeOptions.ContainsKey(SelectedMoveType.Key))
            {
                if(value.Type == AxisType.Rotary)
                {
                    SelectedMoveType = new KeyValuePair<CuePartMoveType, string>(CuePartMoveType.RotaryShortest, "Rotary Shortest");
                }
                else 
                {
                    SelectedMoveType = new KeyValuePair<CuePartMoveType, string>(CuePartMoveType.Linear, "Linear");
                }
            }
        }


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
