using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Core.Models.Sprockets;
using AxisLink.Infrastructure.Sprockets;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AxisLink.Desktop.ViewModels.Windows.ScenerySetup
{
    public partial class ScenerySetupViewModel : ViewModelBase
    {
        private readonly ShowFileManager _fileManager;
        private readonly MotionManager _motionManager;
        private readonly IConsoleLogger _logger;
        private readonly ISprocketProvider _sprocketProvider;

        public ObservableCollection<Sprocket> AvailableHardwareProfiles { get; } = new();
        public Action? RequestClose { get; set; }

        [ObservableProperty]
        private string _windowTitle = "Configure Scenery Object";

        // SCENERY IDENTITY 
        [ObservableProperty]
        private string _sceneryName = string.Empty;

        [ObservableProperty]
        private int _selectedSceneryTypeIndex = 0;

        [ObservableProperty]
        private int _weightKgs = 0;

        // In mm/s or deg/s
        [ObservableProperty]
        private float? _maxSpeed = 250f;

        // TRIMS & LIMITS
        [ObservableProperty]
        private float? _highTrim;

        [ObservableProperty]
        private string _highTrimLabel = string.Empty;

        [ObservableProperty]
        private float? _lowTrim;

        [ObservableProperty]
        private string _lowTrimLabel = string.Empty;

        [ObservableProperty]
        private float? _workingTrim;

        [ObservableProperty]
        private string _workingTrimLabel = string.Empty;

        // HARDWARE PROVISIONING FLAGS & FIELDS
        [ObservableProperty]
        private bool _isOneToOneSceneryToAxis = true;

        // Hardware Profile & Auto-Controller fields
        [ObservableProperty]
        private string _newControllerIp = "192.168.0.10";

        [ObservableProperty]
        private int _newControllerPort = 502;


        [ObservableProperty]
        private Sprocket? _selectedHardwareProfile;

        [ObservableProperty]
        private int _driveChannel = 1;
        private ObservableCollection<int> DisallowedChannels = new();

        [ObservableProperty]
        private float _driveScaleFactor = 1f;

        // mm/rev or deg/rev
        [ObservableProperty]
        private float? _distancePerRevolution = 304.8f;

        public ScenerySetupViewModel(
            ShowFileManager fileManager,
            MotionManager motionManager,
            IConsoleLogger logger,
            ISprocketProvider sprocketProvider)
        {
            _fileManager = fileManager;
            _motionManager = motionManager;
            _logger = logger;
            _sprocketProvider = sprocketProvider;

            InitializeCollections();
        }

        private void InitializeCollections()
        {
            AvailableHardwareProfiles.Clear();
            foreach (var sprocket in _sprocketProvider.GetAvailableSprockets())
            {
                AvailableHardwareProfiles.Add(sprocket);
            }

            // Default selection
            SelectedHardwareProfile = AvailableHardwareProfiles.FirstOrDefault();
            // TODO: Populate AvailableControllers from ShowFileManager / MotionManager
            DisallowedChannels.Clear();
            OnNewControllerIpChanged(NewControllerIp); // Initialize disallowed channels based on the default IP
        }

        partial void OnSelectedHardwareProfileChanged(Sprocket? value)
        {
            if (value != null)
            {
                DriveScaleFactor = value.DefaultDriveScaleFactor;
            }
        }

        partial void OnNewControllerIpChanged(string value)
        {
            var show = _fileManager.CurrentShow;
            if (show == null) return;
            DisallowedChannels.Clear();
            var existingController = show.Controllers?.FirstOrDefault(c => c.IpAddress == NewControllerIp && c.Port == (int)NewControllerPort);
            if (existingController != null)
            {
                // Selected ip of existing controller -> limit hardware options to the protocol of that controller
                SelectedHardwareProfile = AvailableHardwareProfiles.FirstOrDefault(p => p.Protocol == existingController.Protocol);
                AvailableHardwareProfiles.Clear();
                foreach (var sprocket in _sprocketProvider.GetAvailableSprockets().Where(p => p.Protocol == existingController.Protocol))
                {
                    AvailableHardwareProfiles.Add(sprocket);
                }
                var axes = show.Machinery.Axes.Where(a => a.ControllerId == existingController.Id);
                if(axes != null)
                { 
                    // dont allow setting the drive channel to existing drive channel
                    foreach (var axis in show.Machinery.Axes.Where(a => a.ControllerId == existingController.Id))
                    {
                        if(axis.Channel.HasValue)
                        {
                            DisallowedChannels.Add(axis.Channel.Value);
                        }
                        if (DriveChannel == axis.Channel)
                        {
                            DriveChannel++; // Increase to next channel if the current one is already in use
                        }
                    }
                }
            }
            else
            {
                AvailableHardwareProfiles.Clear();
                foreach (var sprocket in _sprocketProvider.GetAvailableSprockets())
                {
                    AvailableHardwareProfiles.Add(sprocket);
                }
            }
        }

        partial void OnDriveChannelChanged(int value)
        {
            if (DisallowedChannels.Contains(value))
            {
                _logger.LogWarning($"Drive Channel {value} is in use on this controller.");
                int candidate = value;
                while (DisallowedChannels.Contains(candidate))
                {
                    candidate++;
                }
                DriveChannel = candidate;
            }
        }
 

        [RelayCommand]
        private void SaveScenery()
        {
            if (string.IsNullOrWhiteSpace(SceneryName))
            {
                _logger.LogWarning("Scenery Name is required.");
                return;
            }

            var show = _fileManager.CurrentShow;
            if (show == null) return;

            // Create a new Controller object or get existing one
            int controllerId;
            var existingController = show.Controllers?.FirstOrDefault(c => c.IpAddress == NewControllerIp && c.Port == (int)NewControllerPort);
            if (existingController != null) {
                controllerId = existingController.Id;
            }
            else {
                if (SelectedHardwareProfile == null)
                {
                    _logger.LogWarning("Please select a valid Hardware Profile / Sprocket.");
                    return;
                }
                controllerId = _motionManager.nextControllerId;
                var newController = new AxisLink.Core.Models.Show.Controller
                {
                    Id = controllerId,
                    Name = $"Controller {controllerId}",
                    IpAddress = NewControllerIp,
                    Port = (int)NewControllerPort,
                    Protocol = SelectedHardwareProfile.Protocol,
                    TimeoutMs = 1000 // Default timeout
                };
                _motionManager.AddNewController(newController);
            }

            // Create a new Axis
            var newAxis = new AxisLink.Core.Models.Show.Axis
            {
                Id = _motionManager.nextAxisId,
                Name = SceneryName,
                ControllerId = controllerId,
                Channel = (int)DriveChannel,
                SprocketId = SelectedHardwareProfile?.Id,
                DriveScaleFactor = DriveScaleFactor,
                DistancePerRevolution = DistancePerRevolution ?? 304.8f,
                MaxSpeed = MaxSpeed ?? 250f,
                HighLimit = HighTrim ?? 0f,
                LowLimit = LowTrim ?? 0f,
            };

            _motionManager.AddNewAxis(newAxis);

            // Create a new Scenery object
            var newScenery = new AxisLink.Core.Models.Show.Scenery
            {
                Id = _motionManager.nextSceneryId,
                Name = SceneryName,
                // Weight is in kg
                Weight = WeightKgs,
                SpeedLimit = MaxSpeed,
                Trims = new AxisLink.Core.Models.Show.SceneryTrims
                {
                    HighTrim = new AxisLink.Core.Models.Show.SceneryLimitTrim{
                        Name = HighTrimLabel,
                        Position = HighTrim
                    },
                    LowTrim = new AxisLink.Core.Models.Show.SceneryLimitTrim{
                        Name = LowTrimLabel,
                        Position = LowTrim
                    },
                    // TODO allow for multiple trims/spikes and loop through to store
                    Trims = new List<AxisLink.Core.Models.Show.SceneryTrim> { 
                        new AxisLink.Core.Models.Show.SceneryTrim{
                        Id = 0, // New trim, ID will be assigned by the system
                        Name = WorkingTrimLabel,
                        Position = WorkingTrim
                    }}
                }
            };
            _motionManager.AddNewScenery(newScenery);

            // Create a new Patch
            var newPatch = new AxisLink.Core.Models.Show.Patch
            {
                Id = newScenery.Id,
                AxisId = newAxis.Id
            };
            _motionManager.AddNewPatch(newPatch);
            _logger.LogInfo("Added Scenery Object: " + SceneryName + " with Axis ID: " + newAxis.Id + " and Controller ID: " + controllerId);
            RequestClose?.Invoke();
        }

        [RelayCommand]
        private void Cancel()
        {
            RequestClose?.Invoke();
        }
    }
}