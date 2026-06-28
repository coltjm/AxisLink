using KinetiCUE.Core.Management;

namespace KinetiCUE.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly MotionManager _motionManager;
        private readonly CueManager _cueManager;

        public MainWindowViewModel(MotionManager motionManager, CueManager cueManager)
        {
            _motionManager = motionManager;
            _cueManager = cueManager;
        }
        public string Greeting { get; } = "Welcome to Avalonia!";
    }
}
