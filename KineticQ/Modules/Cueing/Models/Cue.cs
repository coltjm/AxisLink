using CommunityToolkit.Mvvm.ComponentModel;

namespace KinetiCUE.Modules.Cueing.Models
{
    public enum CueTrigger
    {
        Manual,
        Time,
        Follow
    }

    public partial class Cue : ObservableObject
    {
        // Identification
        [ObservableProperty]
        private string _number = "1";

        [ObservableProperty]
        private string _label = "Deck Move";

        // Timing / Triggering
        [ObservableProperty]
        private CueTrigger _trigger = CueTrigger.Manual;

        [ObservableProperty]
        private float _triggerDelay = 0.0f;

        // Instructions
        // Keeping your Dictionary. 
        // Note: If you add/remove keys dynamically in the UI, 
        // you might eventually need ObservableDictionary or an ObservableCollection wrapper.
        // For now, this is fine for logic.
        public Dictionary<int, MoveInstruction> Instructions { get; set; } = new();
    }
}