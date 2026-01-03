using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KineticQ.Modules.Cueing.Models;
using KineticQ.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace KineticQ.Modules.Cueing.ViewModels
{
    public partial class CueViewModel : ObservableObject
    {
        private readonly Cue _model;
        public Cue Model => _model;
        public ObservableCollection<MoveInstructionViewModel> Instructions { get; } = new();
        public CueViewModel(Cue model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            // Listen for external changes (like undo/redo or file loading)
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
            RefreshInstructions();
        }

        private void RefreshInstructions()
        {
            Instructions.Clear();
            foreach (var kvp in _model.Instructions)
            {
                // Key = MachineID, Value = Instruction
                Instructions.Add(new MoveInstructionViewModel(kvp.Value, kvp.Key, _model));
            }
        }

        // =========================================================
        // PROPERTIES
        // =========================================================

        public string Number
        {
            get => _model.Number;
            set => SetModelProperty(value, (m, v) => m.Number = v);
        }

        public string Label
        {
            get => _model.Label;
            set => SetModelProperty(value, (m, v) => m.Label = v);
        }

        public CueTrigger Trigger
        {
            get => _model.Trigger;
            set => SetModelProperty(value, (m, v) => m.Trigger = v);
        }

        public float TriggerDelay
        {
            get => _model.TriggerDelay;
            set => SetModelProperty(value, (m, v) => m.TriggerDelay = v);
        }

        // TODO: We will handle the 'Instructions' Dictionary here next.
        // It requires a specific ObservableCollection strategy for the Inspector.

        // =========================================================
        // HELPER (Copied from MachineViewModel)
        // =========================================================
        private void SetModelProperty<T>(T value, Action<Cue, T> modelSetter, [CallerMemberName] string? propertyName = null)
        {
            // Reflection to get current value for comparison
            var propInfo = typeof(Cue).GetProperty(propertyName!);
            if (propInfo != null)
            {
                var currentVal = (T)propInfo.GetValue(_model)!;
                if (!EqualityComparer<T>.Default.Equals(currentVal, value))
                {
                    modelSetter(_model, value);
                    OnPropertyChanged(propertyName);
                    FileManager.Instance.MarkAsDirty();
                }
            }
            else
            {
                modelSetter(_model, value);
                OnPropertyChanged(propertyName);
                FileManager.Instance.MarkAsDirty();
            }
        }
        [RelayCommand]
        private void AddInstruction()
        {
            // 1. Default to the first available machine that isn't already in the cue
            var availableMachine = FileManager.Instance.CurrentShow.Machines
                .FirstOrDefault(m => !_model.Instructions.ContainsKey(m.Id));

            if (availableMachine == null) return; // No machines left to add!

            // 2. Create new instruction
            var newInstruction = new MoveInstruction();

            // 3. Add to Model
            _model.Instructions.Add(availableMachine.Id, newInstruction);

            // 4. Add to VM List
            Instructions.Add(new MoveInstructionViewModel(newInstruction, availableMachine.Id, _model));

            FileManager.Instance.MarkAsDirty();
        }

        [RelayCommand]
        private void RemoveInstruction(MoveInstructionViewModel item)
        {
            if (item == null) return;

            // Remove from Model
            if (_model.Instructions.ContainsKey(item.SelectedMachineId))
            {
                _model.Instructions.Remove(item.SelectedMachineId);
            }

            // Remove from UI
            Instructions.Remove(item);
            FileManager.Instance.MarkAsDirty();
        }
    }
}
