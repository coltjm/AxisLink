using CommunityToolkit.Mvvm.ComponentModel;
using KinetiCUE.Modules.Machines.Models;
using KinetiCUE.Services;

namespace KinetiCUE.Modules.Machines.ViewModels
{
    internal partial class MachineViewModel : ObservableObject
    {
        private readonly Machine _model;
        public Machine Model => _model;

        [ObservableProperty]
        private string _units = FileManager.Instance.CurrentShow.DistanceUnits;

        public MachineViewModel(Machine model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            // --- THE LAZY TRICK ---
            // Listen to the Model. If the Model updates "CurrentPosition" (via PLC),
            // automatically tell the View to update "CurrentPosition" too.
            // This replaces your manual Refresh() function.
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        // =========================================================
        // 1. CONFIGURATION PROPERTIES (Read/Write + MarkAsDirty)
        // =========================================================

        public int Id => _model.Id; // Usually Read-Only

        public string Name
        {
            get => _model.Name;
            set => SetModelProperty(value, (m, v) => m.Name = v);
        }

        // --- Mechanics ---

        public bool IsLinear
        {
            get => _model.IsLinear;
            set => SetModelProperty(value, (m, v) => m.IsLinear = v);
        }

        public float MaxVelocity
        {
            get => _model.MaxVelocity;
            set => SetModelProperty(value, (m, v) => m.MaxVelocity = v);
        }

        public float MaxPosition
        {
            get => _model.MaxPosition;
            set => SetModelProperty(value, (m, v) => m.MaxPosition = v);
        }

        public float MinPosition
        {
            get => _model.MinPosition;
            set => SetModelProperty(value, (m, v) => m.MinPosition = v);
        }

        public float HomePosition
        {
            get => _model.Home;
            set => SetModelProperty(value, (m, v) => m.Home = v);
        }

        public float StepsPerRevolution
        {
            get => _model.StepsPerRevolution;
            set => SetModelProperty(value, (m, v) => m.StepsPerRevolution = v);
        }

        public float DistPerRevolution
        {
            get => _model.DistPerRevolution;
            set => SetModelProperty(value, (m, v) => m.DistPerRevolution = v);
        }

        // --- Network ---

        public int PLCId
        {
            get => _model.PLCId;
            set => SetModelProperty(value, (m, v) => m.PLCId = v);
        }

        // --- PLC Registers ---

        public string Addr_MoveCmd
        {
            get => _model.Addr_MoveCmd;
            set => SetModelProperty(value, (m, v) => m.Addr_MoveCmd = v);
        }

        public string Addr_TargetPos
        {
            get => _model.Addr_TargetPos;
            set => SetModelProperty(value, (m, v) => m.Addr_TargetPos = v);
        }

        public string Addr_TargetVel
        {
            get => _model.Addr_TargetVel;
            set => SetModelProperty(value, (m, v) => m.Addr_TargetVel = v);
        }

        public string Addr_CurrentPos
        {
            get => _model.Addr_CurrentPos;
            set => SetModelProperty(value, (m, v) => m.Addr_CurrentPos = v);
        }

        public string Addr_StatusBit
        {
            get => _model.Addr_StatusBit;
            set => SetModelProperty(value, (m, v) => m.Addr_StatusBit = v);
        }

        public string Addr_FaultBit
        {
            get => _model.Addr_FaultBit;
            set => SetModelProperty(value, (m, v) => m.Addr_FaultBit = v);
        }

        // =========================================================
        // 2. LIVE TELEMETRY (Pass-through + Auto-Update)
        // =========================================================
        // Because of the 'PropertyChanged' hook in the constructor,
        // you don't need setters here. When Model updates, UI updates.

        public float CurrentVelocity => _model.CurrentVelocity;
        public float TargetVelocity => _model.TargetVelocity;
        public float CurrentPosition => _model.CurrentPosition;
        public float TargetPosition => _model.TargetPosition;

        public bool IsConnected => _model.IsConnected;
        public bool IsEnabled => _model.IsEnabled;
        public bool IsFaulted => _model.IsFaulted;
        public string FaultMessage => _model.FaultMessage;

        public bool IsAtTargetSpeed => Math.Abs((float)(_model.CurrentVelocity - _model.TargetVelocity)) < 0.1;

        [ObservableProperty] private bool _isSelected;

        // =========================================================
        // 3. HELPER METHODS
        // =========================================================

        /// <summary>
        /// A generic helper to update a Model property, raise notification, and mark file dirty.
        /// eliminates the repetitive 'if(SetProperty...)' block for every field.
        /// </summary>
        /// <summary>
        /// Updates a Model property. Checks if value changed, updates Model, notifies UI, marks Dirty.
        /// </summary>
        private void SetModelProperty<T>(T value, Action<Machine, T> modelSetter, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            // We use reflection to get the current value from the Model to check for equality.
            // This is slightly slower but saves writing 500 lines of code.
            // Given this is UI config (human speed), the performance hit is negligible.

            var propInfo = typeof(Machine).GetProperty(propertyName!);
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
                // Fallback if property name doesn't match (e.g. HomePosition vs Home)
                // Just update and mark dirty.
                modelSetter(_model, value);
                OnPropertyChanged(propertyName);
                FileManager.Instance.MarkAsDirty();
            }
        }
    }
}