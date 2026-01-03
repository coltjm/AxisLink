using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KineticQ.Modules.Core.ViewModels;
using KineticQ.Modules.Core.Views;
using KineticQ.Modules.Machines.ViewModels;
using KineticQ.Modules.Machines.Views;
using KineticQ.Modules.Workspace.ViewModel;
using KineticQ.Modules.Workspace.Views;
using KineticQ.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using KineticQ.Modules.Cueing.Models;
using KineticQ.Modules.Cueing.Views;
using KineticQ.Modules.Cueing.ViewModels;
using KineticQ.Modules.Inspector.Views;
using KinetiCUE.Models;
using Machine = KineticQ.Modules.Machines.Models.Machine;

namespace KineticQ.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _currentCue = 0;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WindowTitle))]
        private string _currentProjectName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WindowTitle))]
        private bool _isDirty;
        [ObservableProperty]
        private object? _selectedContext;
        private ShowFile? _watchedShow;
        public string WindowTitle => $"{CurrentProjectName}{(IsDirty ? "*" : "")} - KinetiCUE";
        public ObservableCollection<Cue> Cues => FileManager.Instance.CurrentShow.Cues;
        public ObservableCollection<Machine> Machines => FileManager.Instance.CurrentShow.Machines;

        public MachineListViewModel MachineListVM { get; }
        public CueListViewModel CueListVM { get; }

        // Display Variables for 4 Quadrant View
        private readonly Dictionary<string, Func<object>> _viewRegistry;

        private object _topLeftView;
        private object _topRightView;
        private object _bottomLeftView;
        private object _bottomRightView;

        public object TopLeftView { get => _topLeftView; set { _topLeftView = value; OnPropertyChanged(); } }
        public object TopRightView { get => _topRightView; set { _topRightView = value; OnPropertyChanged(); } }
        public object BottomLeftView { get => _bottomLeftView; set { _bottomLeftView = value; OnPropertyChanged(); } }
        public object BottomRightView { get => _bottomRightView; set { _bottomRightView = value; OnPropertyChanged(); } }

        private GridLength _row1Height = new GridLength(1, GridUnitType.Star);
        private GridLength _col1Width = new GridLength(1, GridUnitType.Star);

        public GridLength Row1Height { get => _row1Height; set { _row1Height = value; OnPropertyChanged(); } }
        public GridLength Col1Width { get => _col1Width; set { _col1Width = value; OnPropertyChanged(); } }


        public MainViewModel()
        {
            MachineListVM = new MachineListViewModel();
            CueListVM = new CueListViewModel(); 
            _viewRegistry = new Dictionary<string, Func<object>>
            {
                // Syntax: ["SaveName"] = () => new ViewClass(),
                ["CueList"] = () => new CueListView{DataContext = CueListVM},
                ["MachineList"] = () => new MachineListView { DataContext = MachineListVM },
                ["CueStack"] = () => new TextBlock { Text = "CueStack", Foreground = System.Windows.Media.Brushes.White },
                ["Inspector"] = () => new InspectorView {},
                ["Visualizor"] = () => new TextBlock { Text = "Vizualizor", Foreground = System.Windows.Media.Brushes.White },

                // Default fallback
                ["Empty"] = () => null
            };
            FileManager.Instance.PropertyChanged += FileManager_PropertyChanged;
            if (FileManager.Instance.CurrentShow != null)
            {
                FileManager.Instance.CurrentShow.PropertyChanged += CurrentShow_PropertyChanged;
            }
            MachineListVM.SelectionChanged += (obj) =>
            {
                SelectedContext = obj;
            };
            

            // Wire up Selection
            CueListVM.SelectionChanged += (obj) =>
            {
                SelectedContext = obj;
            };
            // Initialize values immediately (so it doesn't wait for the first change)
            CurrentProjectName = FileManager.Instance.CurrentShow.ShowName;
            IsDirty = FileManager.Instance.HasUnsavedChanges;
            LoadLayout();
        }

        private void FileManager_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // We check "Which property changed?" using e.PropertyName
            if (e.PropertyName == nameof(FileManager.HasUnsavedChanges))
            {
                // Sync the Singleton value to the ViewModel value
                IsDirty = FileManager.Instance.HasUnsavedChanges;
            }

            else if (e.PropertyName == nameof(FileManager.CurrentShow))
            {
                SubscribeToNewShow();
            }
        }
        private void SubscribeToNewShow()
        {
            // 1. Unhook old one
            if (_watchedShow != null)
            {
                _watchedShow.PropertyChanged -= CurrentShow_PropertyChanged;
            }

            // 2. Update reference
            _watchedShow = FileManager.Instance.CurrentShow;

            // 3. Hook new one
            if (_watchedShow != null)
            {
                _watchedShow.PropertyChanged += CurrentShow_PropertyChanged;

                // Refresh UI immediately
                CurrentProjectName = _watchedShow.ShowName;
            }
        }
        private void CurrentShow_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShowFile.ShowName))
            {
                // Update the UI
                if (FileManager.Instance.CurrentShow != null)
                {
                    CurrentProjectName = FileManager.Instance.CurrentShow.ShowName;
                }
            }
        }

        private object GetViewByKey(string key)
        {
            // 1. Check if the key exists in our registry
            if (!string.IsNullOrEmpty(key) && _viewRegistry.ContainsKey(key))
            {
                // 2. Invoke the function to create a NEW instance of that view
                return _viewRegistry[key].Invoke();
            }

            // 3. Fallback if the save file has a weird name (prevents crashing)
            return new TextBlock
            {
                Text = $"Unknown View: {key}",
                Foreground = System.Windows.Media.Brushes.Red,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        public void LoadLayout()
        {
            // No switch statements!
            TopLeftView = GetViewByKey(FileManager.Instance.CurrentShow.TlWindow);
            TopRightView = GetViewByKey(FileManager.Instance.CurrentShow.TrWindow);
            BottomLeftView = GetViewByKey(FileManager.Instance.CurrentShow.BlWindow);
            BottomRightView = GetViewByKey(FileManager.Instance.CurrentShow.BrWindow);
        }


        private void PromptSave()
        {
            // Prompt user to save unsaved changes before closing/opening new file
            var vm = new UnsavedChangesViewModel();
            var window = new UnsavedChangesWindow();
            window.DataContext = vm;
            window.Owner = Application.Current.MainWindow;
            var result = window.ShowDialog();
            if (result == true)
            {
                Save();
            }
            return;
        }


        // --------------- FILE MENU COMMANDS ------------------------
        #region File Menu Commands
        [RelayCommand]
        private void NewProject()
        {
            // Prompt To save current project if dirty
            // set current show instance to blank one
            // Open the project config window
            if (FileManager.Instance.HasUnsavedChanges)
            {
                PromptSave();
            }
            FileManager.Instance.NewShow();
            ConfigureProject();
        }

        [RelayCommand]
        private void Save()
        {
            // SCENARIO 1: FIRST SAVE (Acting as "Save As")
            if (string.IsNullOrEmpty(FileManager.Instance.CurrentFilePath))
            {
                SaveAs();
                return;
            }

            // SCENARIO 2: SUBSEQUENT SAVE (Prompt for details)
            var vm = new KineticQ.Modules.Core.ViewModels.SavePromptViewModel();

            var prompt = new KineticQ.Modules.Core.Views.SavePromptWindow();

            prompt.DataContext = vm;
            prompt.Owner = Application.Current.MainWindow;


            if (prompt.ShowDialog() == true)
            {
                Debug.WriteLine("dialog closed");
                Debug.WriteLine($"User confirmed save: Label={vm.VersionLabel}, Milestone={vm.IsMilestone}");
                if (string.IsNullOrEmpty(vm.VersionLabel))
                {
                    vm.VersionLabel = "autosave" + DateTime.Now.ToString("_yyyyMMdd_HHmmss");
                }
                FileManager.Instance.CurrentShow.IsMilestone = vm.IsMilestone;
                // Pass these details to your FileManager
                FileManager.Instance.SaveShow(FileManager.Instance.CurrentFilePath, vm.VersionLabel, vm.IsMilestone);

                MessageBox.Show($"Saved: {vm.VersionLabel}");
            }
            else
            {
                Debug.WriteLine("User canceled save.");
            }




        }

        [RelayCommand]
        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "KinetiCUE Show Files (*.kcue)|*.kcue|All files (*.*)|*.*",
                DefaultExt = "kcue",
                FileName = $"{FileManager.Instance.CurrentShow.ShowName}.kcue",
                RestoreDirectory = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FileManager.Instance.SaveShow(saveFileDialog.FileName, "Initial Save", isMilestone: true);
            }
        }

        [RelayCommand]
        private void Load()
        {

            if (FileManager.Instance.HasUnsavedChanges)
            {
                PromptSave();
            }

            Debug.WriteLine("Loading");
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "KinetiCUE Show Files (*.kcue)|*.kcue|All files (*.*)|*.*",
                DefaultExt = "kcue",
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FileManager.Instance.LoadShow(openFileDialog.FileName);
                OnPropertyChanged(nameof(Cues));
                OnPropertyChanged(nameof(Machines));
                LoadLayout();
            }
        }


        [RelayCommand]
        private void Exit()
        {
            if (FileManager.Instance.HasUnsavedChanges)
            {
                PromptSave();
            }
            Application.Current.Shutdown();
        }
        #endregion
        // --------------- EDIT MENU COMMANDS ------------------------
        #region Edit Menu Commands
        [RelayCommand]
        private void EditMachine(Machine machineToEdit)
        {
            // 1. Create the VM
            var vm = new MachineConfigViewModel(machineToEdit);

            // 2. Create the Window
            var window = new KineticQ.Modules.Machines.Views.MachineConfigWindow();

            // 3. Link them
            window.DataContext = vm;

            // 4. Hook up the Close Action so the VM can close the window
            vm.RequestClose = () => window.Close();

            // 5. Show as Dialog (blocks interaction with main window until closed)
            window.ShowDialog();
        }

        [RelayCommand]
        private void NewMachine()
        {
            // 1. Create the VM
            var vm = new MachineConfigViewModel();

            // 2. Create the Window
            var window = new KineticQ.Modules.Machines.Views.MachineConfigWindow();

            // 3. Link them
            window.DataContext = vm;

            window.Owner = Application.Current.MainWindow;

            // 4. Hook up the Close Action so the VM can close the window
            vm.RequestClose = () => window.Close();

            // 5. Show as Dialog (blocks interaction with main window until closed)
            window.ShowDialog();

        }

        [RelayCommand]
        private void ViewMachineList()
        {
            //TODO
            return;
        }

        [RelayCommand]
        private void ConfigureProject()
        {
            var vm = new ProjectSetupViewModel();
            var setupWindow = new ProjectSetupWindow();
            setupWindow.DataContext = vm;
            setupWindow.Owner = Application.Current.MainWindow;
            var result = setupWindow.ShowDialog();
            if (result == true)
            {
                //FileManager.Instance.MarkAsDirty();
            }
        }
        #endregion
        // --------------- VIEW MENU COMMANDS ------------------------
        #region View Menu Commands
        [RelayCommand]
        private void SetLayoutProgramming()
        {
            // Define what goes where for "Programming"
            FileManager.Instance.CurrentShow.TlWindow = "MachineList";
            FileManager.Instance.CurrentShow.TrWindow = "Inspector";
            FileManager.Instance.CurrentShow.BlWindow = "CueList";
            FileManager.Instance.CurrentShow.BrWindow = "LogTerminal";
            LoadLayout();
        }

        [RelayCommand]
        private void SetLayoutShowControl()
        {
            // Define what goes where for "Show Mode"
            FileManager.Instance.CurrentShow.TlWindow = "CueList";
            FileManager.Instance.CurrentShow.TrWindow = "MachineList";
            FileManager.Instance.CurrentShow.BlWindow = "CueStack";
            FileManager.Instance.CurrentShow.BrWindow = "Visualizer";
            LoadLayout();
        }

        [RelayCommand]
        private void ResetSpacing()
        {
            Row1Height = new GridLength(1, GridUnitType.Star);
            Col1Width = new GridLength(1, GridUnitType.Star);
        }

        [RelayCommand]
        private void CustomizeLayout()
        {
            var vm = new CustomizeLayoutViewModel();
            var window = new CustomizeLayoutWindow();
            window.DataContext = vm;

        }

        #endregion
        // --------------- WINDOW COMMANDS ------------------------
        #region Window Commands
        [RelayCommand]
        private void Go()
        {
            //TODO
            return;
        }

        [RelayCommand]
        private void Stop()
        {
            //TODO
            return;
        }
        #endregion
    }
}