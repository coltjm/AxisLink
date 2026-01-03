using CommunityToolkit.Mvvm.ComponentModel;
using KinetiCUE.Models;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.Json;


namespace KineticQ.Services
{
    public partial class FileManager : ObservableObject
    {
        // create singleton
        private static FileManager? _instance;

        public static FileManager Instance => _instance ??= new FileManager();


        [ObservableProperty]
        private ShowFile _currentShow;
        [ObservableProperty]
        private string _currentFilePath = "";
        private bool _hasUnsavedChanges = false;
        public bool HasUnsavedChanges
        {
            get => _hasUnsavedChanges;
            private set
            {
                _hasUnsavedChanges = value;
                // Notify UI to enable/disable the "Save" button asterisk (*)
                OnPropertyChanged(nameof(HasUnsavedChanges));
            }
        }


        // CONSTANTS 
        private const string CurrentFileName = "current.json";
        private const string ManifestFileName = "manifest.json";
        private const string HistoryFolder = "History";

        private FileManager()
        {
            _currentShow = new ShowFile();
        }


        // Call when any changes are made
        public void MarkAsDirty()
        {
            if (!HasUnsavedChanges)
            {
                HasUnsavedChanges = true;

            }
        }

        public void NewShow()
        {
            CurrentShow = new ShowFile();
        }

        public void SaveShow(string filePath, string userLabel = "", bool isMilestone = false)
        {
            Debug.WriteLine(CurrentShow.Revision);
            // Update the Data Model
            CurrentShow.Revision++;
            CurrentShow.Label = userLabel;

            // Prepare Temp Directory
            string tempPath = Path.Combine(Path.GetTempPath(), "KinetiCUE_Saving");
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);

            // Handle Existing History (If overwriting a file)
            Manifest manifest = new Manifest();

            if (File.Exists(filePath))
            {
                try
                {
                    // Unzip old file to temp to preserve its history folder
                    ZipFile.ExtractToDirectory(filePath, tempPath);

                    // Load existing manifest
                    string manifestPath = Path.Combine(tempPath, ManifestFileName);
                    if (File.Exists(manifestPath))
                    {
                        string json = File.ReadAllText(manifestPath);
                        manifest = JsonSerializer.Deserialize<Manifest>(json) ?? new Manifest();
                    }

                    // Move the OLD 'current.json' into 'History'
                    string oldCurrent = Path.Combine(tempPath, CurrentFileName);
                    if (File.Exists(oldCurrent))
                    {
                        string jsonString = File.ReadAllText(oldCurrent);
                        var loadedShow = JsonSerializer.Deserialize<ShowFile>(jsonString);
                        string historyDir = Path.Combine(tempPath, HistoryFolder);
                        Directory.CreateDirectory(historyDir);

                        // Filename: History/{SHOW NAME}_{MILESTONE}_{LABEL}.json
                        // OR History/{SHOW NAME}_rev{#}.json
                        string backupName = loadedShow.IsMilestone ? $"{loadedShow.ShowName}_{loadedShow?.Label}.json" : $"{loadedShow.ShowName}_rev{manifest.NextRevisionId}.json";
                        string backupPath = Path.Combine(historyDir, backupName);

                        // Move and overwrite if exists
                        if (File.Exists(backupPath)) File.Delete(backupPath);
                        File.Move(oldCurrent, backupPath);

                        // Add to Manifest
                        manifest.Snapshots.Add(new HistoryEntry
                        {
                            Id = manifest.NextRevisionId,
                            Timestamp = DateTime.Now,
                            Label = loadedShow.Label,
                            Filename = $"{HistoryFolder}/{backupName}",
                            IsMilestone = loadedShow.IsMilestone
                        });

                        manifest.NextRevisionId = CurrentShow.Revision;
                    }
                }
                catch (Exception ex)
                {
                    // If unzip fails (corrupt), we proceed with a fresh save
                    System.Diagnostics.Debug.WriteLine($"Error reading old file: {ex.Message}");
                }
            }

            // Write NEW Data
            var options = new JsonSerializerOptions { WriteIndented = true };

            // Write current.json
            string newJson = JsonSerializer.Serialize(CurrentShow, options);

            File.WriteAllText(Path.Combine(tempPath, CurrentFileName), newJson);

            // Write manifest.json
            string manifestJson = JsonSerializer.Serialize(manifest, options);
            File.WriteAllText(Path.Combine(tempPath, ManifestFileName), manifestJson);

            // Zip it up
            if (File.Exists(filePath)) File.Delete(filePath);
            ZipFile.CreateFromDirectory(tempPath, filePath);
            Debug.WriteLine($"Show saved to {filePath}");
            // Cleanup
            Directory.Delete(tempPath, true);
            HasUnsavedChanges = false;
            this.CurrentFilePath = filePath;
        }

        public void LoadShow(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string tempPath = Path.Combine(Path.GetTempPath(), "KinetiCUE_Loading");
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);

            try
            {
                // Unzip
                ZipFile.ExtractToDirectory(filePath, tempPath);

                // Read current.json
                string jsonPath = Path.Combine(tempPath, CurrentFileName);
                if (File.Exists(jsonPath))
                {
                    string jsonString = File.ReadAllText(jsonPath);
                    var loadedShow = JsonSerializer.Deserialize<ShowFile>(jsonString);

                    if (loadedShow != null)
                    {
                        this.CurrentFilePath = filePath;
                        CurrentShow = loadedShow;
                        this.HasUnsavedChanges = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle corruption
                System.Diagnostics.Debug.WriteLine($"Load Error: {ex.Message}");
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            }
        }

        public int GetNextMachineId()
        {
            if (CurrentShow.Machines.Count == 0) return 0;
            return CurrentShow.Machines.Max(m => m.Id) + 1;
        }
    }

    // HELPER CLASSES FOR THE MANIFEST
    public class Manifest
    {
        public int NextRevisionId { get; set; } = 1;
        public List<HistoryEntry> Snapshots { get; set; } = new List<HistoryEntry>();
    }

    public class HistoryEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Label { get; set; }
        public bool IsMilestone { get; set; } = false;
        public string Filename { get; set; }
    }
}