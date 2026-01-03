using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KineticQ.Modules.Cueing.Models;
using KineticQ.Services;
using KinetiCUE.Models;

namespace KineticQ.Modules.Cueing.ViewModels
{
    public partial class CueListViewModel : ObservableObject
    {
        // Bind the View to this wrapper list
        public ObservableCollection<CueViewModel> Cues { get; } = new();

        [ObservableProperty]
        private CueViewModel? _selectedCue;

        // Standard C# Event for MainViewModel to listen to
        public event Action<object?>? SelectionChanged;

        partial void OnSelectedCueChanged(CueViewModel? value)
        {
            SelectionChanged?.Invoke(value);
        }

        public CueListViewModel()
        {
            HookIntoShow(FileManager.Instance.CurrentShow);

            FileManager.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(FileManager.CurrentShow))
                {
                    HookIntoShow(FileManager.Instance.CurrentShow);
                }
            };
        }

        private void HookIntoShow(ShowFile show)
        {
            Cues.Clear();
            if (show == null) return;

            // A. Populate wrappers
            foreach (var model in show.Cues)
            {
                Cues.Add(new CueViewModel(model));
            }

            // B. Sync with Model changes
            show.Cues.CollectionChanged += Cues_CollectionChanged;
        }

        private void Cues_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // Exact same logic as MachineListViewModel
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (Cue newModel in e.NewItems)
                            Cues.Add(new CueViewModel(newModel));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (Cue oldModel in e.OldItems)
                        {
                            var vm = Cues.FirstOrDefault(x => x.Model == oldModel);
                            if (vm != null) Cues.Remove(vm);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Cues.Clear();
                    break;
            }
        }

        [RelayCommand]
        private void AddCue()
        {
            // Add directly to the Model. The hook above updates the UI.
            // Logic to guess next number
            string nextNum = (Cues.Count + 1).ToString();

            var newCue = new Cue { Number = nextNum, Label = "New Cue" };
            FileManager.Instance.CurrentShow.Cues.Add(newCue);
            FileManager.Instance.MarkAsDirty();

            // Optional: Auto-select the new wrapper
            // We have to wait for the UI list to update, or find it manually
            // A simple hack:
            var newWrapper = Cues.FirstOrDefault(c => c.Model == newCue);
            if (newWrapper != null) SelectedCue = newWrapper;
        }

        [RelayCommand]
        private void DeleteCue()
        {
            if (SelectedCue != null)
            {
                FileManager.Instance.CurrentShow.Cues.Remove(SelectedCue.Model);
                FileManager.Instance.MarkAsDirty();
            }
        }
    }
}