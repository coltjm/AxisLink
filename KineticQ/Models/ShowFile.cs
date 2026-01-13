using CommunityToolkit.Mvvm.ComponentModel;
using KinetiCUE.Modules.Machines.Models;
using KinetiCUE.Services;
using System.Collections.ObjectModel;
using KinetiCUE.Modules.Cueing.Models;
using KinetiCUE.Modules.PLC.Models;
using Cue = KinetiCUE.Modules.Cueing.Models.Cue;
using FileManager = KinetiCUE.Services.FileManager;
using Machine = KinetiCUE.Modules.Machines.Models.Machine;

namespace KinetiCUE.Models
{
    public partial class ShowFile : ObservableObject
    {
        // METADATA
        [ObservableProperty]
        private string _showName = "New Show";
        [ObservableProperty]
        private int _revision = 0;
        [ObservableProperty]
        private string _author = "Programmer";
        [ObservableProperty]
        private DateTime _lastSaved = DateTime.Now;
        [ObservableProperty]
        private string _label = "Initial save";
        [ObservableProperty]
        private bool _isMilestone = false;
        [ObservableProperty]
        private string _distanceUnits = "in";
        // Workspace Layout [CueList, MachineList, CueStack, Inspector, Vizualization]
        [ObservableProperty]
        private string _tlWindow = "MachineList";
        [ObservableProperty]
        private string _trWindow = "Inspector";
        [ObservableProperty]
        private string _blWindow = "CueList";
        [ObservableProperty]
        private string _brWindow = "LogTerminal";

        public ObservableCollection<Machine> Machines { get; set; } = new ObservableCollection<Machine>();
        public ObservableCollection<Cue> Cues { get; set; } = new ObservableCollection<Cue>();
        public ObservableCollection<PLCModel> PLCs { get; set; } = new ObservableCollection<PLCModel>();

        // CONSTRUCTOR
        public ShowFile()
        {
            Machines.CollectionChanged += (s, e) => FileManager.Instance.MarkAsDirty();
            Cues.CollectionChanged += (s, e) => FileManager.Instance.MarkAsDirty();
            PLCs.CollectionChanged += (s, e) => FileManager.Instance.MarkAsDirty();
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            // 1. Let the base class do its normal work (updating bindings)
            base.OnPropertyChanged(e);

            // 2. EXCLUSION LIST:
            // We do NOT want to mark the file dirty if we are just updating the timestamp 
            // immediately after a save. Otherwise, the file will always look "Unsaved."
            if (e.PropertyName == nameof(LastSaved))
                return;

            // 3. Mark as Dirty
            // This makes sure editing _showName, _author, or _tlWindow triggers the flag.
            FileManager.Instance.MarkAsDirty();
        }

    }
}