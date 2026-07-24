using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Configs;
using AxisLink.Core.Models.Show;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Management
{
    public class ShowFileManager
    {
        // Holds show file info
        private readonly IShowFileStorage _storage;
        public ShowFile CurrentShow { get; private set; }

        public List<Axis> axes;

        public ShowFileManager(IShowFileStorage storage)
        {
            // Get File Storage service from DI
            _storage = storage;
        }

        public ShowFileManager()
        {
        }

        // Opens a show file from the specified path and loads it into CurrentShow
        public void OpenShow(string path)
        {
            CurrentShow = _storage.LoadFromFileSystem(path);
        }

        

        // Saves the current show file to the specified path
        public void SaveShow(string path)
        {
            if (CurrentShow == null)
                throw new InvalidOperationException("No show is currently loaded to save.");
            _storage.SaveToFileSystem(CurrentShow, path);
        }


        // Creates a new show file and sets it as the current show
        public void NewShow()
        {
            // Prompt user to save current show if it has unsaved changes (not implemented here)
            // create empty collections to avoid null reference exceptions
            List<Controller> controllers = new List<Controller>();
            List<Axis> Axes = new List<Axis>();
            List<Group> Groups = new List<Group>();
            List<Scenery> Scenery = new List<Scenery>();
            List<Patch> Patches = new List<Patch>();
            List<Cue> Cues = new List<Cue>();
            // In the future, set default project config from user preferences
            ProjectConfig projectConfig = new ProjectConfig();

            CurrentShow = new ShowFile(controllers, Axes, Groups, Scenery, Patches, Cues, projectConfig);
            
        }

    }
}
