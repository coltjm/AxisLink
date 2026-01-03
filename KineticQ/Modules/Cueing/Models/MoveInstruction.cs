using CommunityToolkit.Mvvm.ComponentModel;

namespace KinetiCUE.Modules.Cueing.Models
{
    // Must be partial for the source generator to work
    public partial class MoveInstruction : ObservableObject
    {
        [ObservableProperty]
        private float _targetPosition;

        [ObservableProperty]
        private float _duration = 5.0f;

        [ObservableProperty]
        private float _velocity = 0.0f;

        [ObservableProperty]
        private float _acceleration = 0.0f;

        [ObservableProperty]
        private float _deceleration = 0.0f;
    }
}