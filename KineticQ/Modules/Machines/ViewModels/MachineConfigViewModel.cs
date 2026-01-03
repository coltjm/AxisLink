using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinetiCUE.Services;
using Machine = KinetiCUE.Modules.Machines.Models.Machine;

namespace KinetiCUE.Modules.Machines.ViewModels
{
    public partial class MachineConfigViewModel : ObservableObject
    {
        private readonly Machine _originalModel;

        // This Action allows the ViewModel to tell the Window to close
        public Action? RequestClose { get; set; }

        // --- TEMPORARY EDITING FIELDS ---
        [ObservableProperty] private int _id;
        [ObservableProperty] private string _name;
        [ObservableProperty] private float _maxVelocity;
        [ObservableProperty] private float _maxPosition;
        [ObservableProperty] private float _minPosition;
        [ObservableProperty] private float _homePosition;
        [ObservableProperty] private float _stepsPerRevolution;
        [ObservableProperty] private float _distPerRevolution;
        [ObservableProperty] private bool _isLinear;
        [ObservableProperty] private string _iPAddress;
        [ObservableProperty] private int _port;
        [ObservableProperty] private int _unitId;
        [ObservableProperty] private string _addr_MoveCmd;
        [ObservableProperty] private string _addr_TargetPos;
        [ObservableProperty] private string _addr_TargetVel;
        [ObservableProperty] private string _addr_CurrentPos;
        [ObservableProperty] private string _addr_StatusBit;
        [ObservableProperty] private string _addr_FaultBit;

        public MachineConfigViewModel(Machine machine)
        {
            _originalModel = machine;

            // 1. COPY DATA (Load Phase)
            // We copy values from the Model to these ViewModel properties
            // so editing the textbox doesn't instantly change the live show file.
            Id = machine.Id;
            Name = machine.Name;
            MaxVelocity = machine.MaxVelocity;
            MaxPosition = machine.MaxPosition;
            MinPosition = machine.MinPosition;
            HomePosition = machine.Home;
            StepsPerRevolution = machine.StepsPerRevolution;
            DistPerRevolution = machine.DistPerRevolution;
            IsLinear = machine.IsLinear;
            IPAddress = machine.IPAddress;
            Port = machine.Port;
            UnitId = machine.UnitId;
            Addr_MoveCmd = machine.Addr_MoveCmd;
            Addr_TargetPos = machine.Addr_TargetPos;
            Addr_TargetVel = machine.Addr_TargetVel;
            Addr_CurrentPos = machine.Addr_CurrentPos;
            Addr_StatusBit = machine.Addr_StatusBit;
            Addr_FaultBit = machine.Addr_FaultBit;

        }

        public MachineConfigViewModel()
        {
            Id = FileManager.Instance.GetNextMachineId();
            Name = $"Machine {Id}";
            MaxVelocity = 10;
            MaxPosition = 1000;
            MinPosition = 0;
            HomePosition = 0;
            StepsPerRevolution = 200;
            DistPerRevolution = 10;
            IsLinear = true;
            IPAddress = "";
            Port = 0;
            UnitId = 0;
            Addr_MoveCmd = "";
            Addr_TargetPos = "";
            Addr_TargetVel = "";
            Addr_CurrentPos = "";
            Addr_StatusBit = "";
            Addr_FaultBit = "";
        }

        [RelayCommand]
        private void Save()
        {

            if (_originalModel != null)
            {

                // 2. SAVE DATA (Commit Phase)
                // Push values back to the original model
                _originalModel.Name = Name;
                _originalModel.MaxVelocity = MaxVelocity;
                _originalModel.MaxPosition = MaxPosition;
                _originalModel.MinPosition = MinPosition;
                _originalModel.Home = HomePosition;
                _originalModel.StepsPerRevolution = StepsPerRevolution;
                _originalModel.DistPerRevolution = DistPerRevolution;
                _originalModel.IsLinear = IsLinear;
                _originalModel.IPAddress = IPAddress;
                _originalModel.Port = Port;
                _originalModel.UnitId = UnitId;
                _originalModel.Addr_MoveCmd = Addr_MoveCmd;
                _originalModel.Addr_TargetPos = Addr_TargetPos;
                _originalModel.Addr_TargetVel = Addr_TargetVel;
                _originalModel.Addr_CurrentPos = Addr_CurrentPos;
                _originalModel.Addr_StatusBit = Addr_StatusBit;
                _originalModel.Addr_FaultBit = Addr_FaultBit;



            }
            else
            {
                Machine newMachine = new Machine(
                    Id, Name, MaxVelocity,
                    MaxPosition, MinPosition, HomePosition,
                    StepsPerRevolution, DistPerRevolution,
                    IsLinear,
                    IPAddress,
                    Port,
                    UnitId,
                    Addr_MoveCmd,
                    Addr_TargetPos,
                    Addr_TargetVel,
                    Addr_CurrentPos,
                    Addr_StatusBit,
                    Addr_FaultBit);
                FileManager.Instance.CurrentShow.Machines.Add(newMachine);
            }
            // Mark file as dirty
            //FileManager.Instance.MarkAsDirty();

            // Close Window
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            // Just close without saving
            RequestClose?.Invoke();
        }
    }
}