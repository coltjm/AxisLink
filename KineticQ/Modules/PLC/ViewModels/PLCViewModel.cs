using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KinetiCUE.Modules.PLC.Models;
using KinetiCUE.Services;


namespace KinetiCUE.Modules.PLC.ViewModels
{
    public partial class PLCViewModel : ObservableObject
    {
        private readonly PLCModel _originalModel;

        // This Action allows the ViewModel to tell the Window to close
        public Action? RequestClose { get; set; }

        // --- TEMPORARY EDITING FIELDS ---
        [ObservableProperty] private int _id;
        [ObservableProperty] private string _name;
        [ObservableProperty] private string _iPAddress;
        [ObservableProperty] private int _port;
        [ObservableProperty] private int _unitId;
        [ObservableProperty] private int _eStopInputAddress;

        public PLCViewModel(PLCModel plc)
        {
            _originalModel = plc;

            // 1. COPY DATA (Load Phase)
            // We copy values from the Model to these ViewModel properties
            // so editing the textbox doesn't instantly change the live show file.
            Id = plc.Id;
            Name = plc.Name;
            IPAddress = plc.IpAddress;
            Port = plc.Port;
            UnitId = plc.UnitId;
            EStopInputAddress = plc.EStopSignalAddress;

        }

        public PLCViewModel()
        {
            Id = FileManager.Instance.GetNextPLCId();
            Name = $"PLC {Id}";
            IPAddress = "192.168.0.10";
            Port = 502;
            UnitId = 0;
            
        }

        [RelayCommand]
        private void Save()
        {

            if (_originalModel != null)
            {

                // 2. SAVE DATA (Commit Phase)
                // Push values back to the original model
                _originalModel.Name = Name;
                _originalModel.IpAddress = IPAddress;
                _originalModel.Port = Port;
                _originalModel.UnitId = UnitId;
                



            }
            else
            {
                PLCModel newPLC = new PLCModel(
                    Id, Name, IPAddress,Port,
                    UnitId, EStopInputAddress);
                FileManager.Instance.CurrentShow.PLCs.Add(newPLC);
            }

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