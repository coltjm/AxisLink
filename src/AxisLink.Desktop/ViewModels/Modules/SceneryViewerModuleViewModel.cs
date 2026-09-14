using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Desktop.Utilities;
using System.Collections.ObjectModel;
using AxisLink.Core.Models.Show;

namespace AxisLink.Desktop.ViewModels.Modules
{
    public partial class SceneryViewerModuleViewModel : ModuleViewModelBase
    {
        private readonly ShowFileManager _fileManager;
        private readonly MotionManager _motionManager;
        private readonly WindowManager _windowManager;
        private readonly IConsoleLogger _logger;

        public ObservableCollection<SceneryCardViewModel> DisplayScenery { get; } = new();

        public SceneryViewerModuleViewModel(
            ShowFileManager fileManager,
            MotionManager motionManager,
            WindowManager windowManager,
            IConsoleLogger logger)
        {
            _fileManager = fileManager;
            _motionManager = motionManager;
            _windowManager = windowManager;
            _logger = logger;
            Title = "Scenery Viewer";

            RebuildSceneryCards();

            // Hook to show file and engine updates
            _motionManager.SceneryAdded += OnSceneryChanged;
            _motionManager.SceneryRemoved += OnSceneryChanged;
        }

        private void RebuildSceneryCards()
        {
            DisplayScenery.Clear();

            var currentShow = _fileManager.CurrentShow;
            if (currentShow?.Machinery.Scenery == null) return;

            foreach (var sceneryObj in currentShow.Machinery.Scenery)
            {
                var card = new SceneryCardViewModel(sceneryObj, _motionManager, _fileManager, _windowManager);
                DisplayScenery.Add(card);
            }
        }

        private void OnShowReloaded(object? sender, System.EventArgs e)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(RebuildSceneryCards);
        }

        private void OnSceneryChanged(Scenery scenery)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(RebuildSceneryCards);
        }


        public override void Dispose()
        {
            //_fileManager.ShowFileLoaded -= OnShowReloaded;
            base.Dispose();
        }
    }
}