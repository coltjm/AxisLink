using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KineticQ.Modules.Cueing.Models;
using KineticQ.Modules.Machines.Models; // For Machine model
using KineticQ.Services;
using System.Linq;

namespace KineticQ.Modules.Cueing.ViewModels
{
    public partial class MoveInstructionViewModel : ObservableObject
    {
        private readonly MoveInstruction _model;
        private readonly Cue _parentCue;
        private int _machineId;

        public MoveInstructionViewModel(MoveInstruction model, int machineId, Cue parentCue)
        {
            _model = model;
            _machineId = machineId;
            _parentCue = parentCue;
        }

        // =========================================================
        // MACHINE SELECTION (The Complex Part)
        // =========================================================

        // This gives the ComboBox a list of all available machines in the project
        public System.Collections.ObjectModel.ObservableCollection<Machine> AvailableMachines
            => FileManager.Instance.CurrentShow.Machines;

        public int SelectedMachineId
        {
            get => _machineId;
            set
            {
                if (_machineId != value)
                {
                    // SWAP KEYS IN DICTIONARY
                    // 1. Remove old key
                    if (_parentCue.Instructions.ContainsKey(_machineId))
                    {
                        _parentCue.Instructions.Remove(_machineId);
                    }

                    // 2. Update local ID
                    _machineId = value;

                    // 3. Add to new key
                    if (!_parentCue.Instructions.ContainsKey(_machineId))
                    {
                        _parentCue.Instructions.Add(_machineId, _model);
                    }

                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MachineName));
                    FileManager.Instance.MarkAsDirty();
                }
            }
        }

        // =========================================================
        // INSTRUCTION PROPERTIES
        // =========================================================

        public float TargetPosition
        {
            get => _model.TargetPosition;
            set
            {
                if (_model.TargetPosition != value)
                {
                    _model.TargetPosition = value;
                    OnPropertyChanged();
                    FileManager.Instance.MarkAsDirty();
                }
            }
        }

        public float Duration
        {
            get => _model.Duration;
            set
            {
                if (_model.Duration != value)
                {
                    _model.Duration = value;
                    OnPropertyChanged();
                    FileManager.Instance.MarkAsDirty();
                }
            }
        }

        public string MachineName
        {
            get
            {
                var machine = AvailableMachines.FirstOrDefault(m => m.Id == SelectedMachineId);
                return machine?.Name ?? $"Unknown (ID: {SelectedMachineId})";
            }
        }

       
        // Add Velocity/Accel/Decel here if needed similarly...
    }
}