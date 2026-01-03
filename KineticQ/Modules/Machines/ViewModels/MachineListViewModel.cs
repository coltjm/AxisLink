using CommunityToolkit.Mvvm.ComponentModel;
using KineticQ.Services;
using KineticQ.Modules.Machines.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KinetiCUE.Models; // Needed for SingleOrDefault

namespace KineticQ.Modules.Machines.ViewModels
{
    internal partial class MachineListViewModel : ObservableObject
    {
        // The list the View binds to
        public ObservableCollection<MachineViewModel> Machines { get; } = new();
        [ObservableProperty]
        private MachineViewModel? _selectedMachine;
        partial void OnSelectedMachineChanged(MachineViewModel? value)
        {
            // We need a way to reach MainViewModel. 
            // Ideally, MainViewModel listens to us, or we inject a service.
            // For simplicity in this architecture, we can expose an event.
            SelectionChanged?.Invoke(value);
        }

        public event Action<object?>? SelectionChanged;
        public MachineListViewModel()
        {
            // 1. Initial Load
            HookIntoShow(FileManager.Instance.CurrentShow);

            // 2. Listen for "New Project" or "Load Project" events
            // Since this VM stays alive, we need to know when the ShowFile object is swapped out.
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
            // Clear existing UI items from the old show
            Machines.Clear();

            if (show == null) return;

            // A. Populate from the new show
            foreach (var model in show.Machines)
            {
                Machines.Add(new MachineViewModel(model));
            }

            // B. Subscribe to the new show's list
            // (Ideally, you should unsubscribe from the old one first to be perfect, 
            // but for a singleton app, this is usually fine)
            show.Machines.CollectionChanged += Machines_CollectionChanged;
        }

        // This runs AUTOMATICALLY whenever FileManager.Instance.CurrentShow.Machines changes
        private void Machines_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Create a wrapper for the new model and add to UI
                    foreach (Machine newModel in e.NewItems!)
                    {
                        Machines.Add(new MachineViewModel(newModel));
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    // Find the wrapper corresponding to the removed model and delete it
                    foreach (Machine oldModel in e.OldItems!)
                    {
                        var vmToRemove = Machines.FirstOrDefault(vm => vm.Model == oldModel);
                        if (vmToRemove != null)
                        {
                            Machines.Remove(vmToRemove);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    // Happens if you call .Clear() on the model list
                    Machines.Clear();
                    break;
            }
        }
    }
}