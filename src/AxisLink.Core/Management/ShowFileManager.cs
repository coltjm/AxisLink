using AxisLink.Core.Interfaces;
using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Management
{
    public class ShowFileManager
    {
        // Holds show file info
        private readonly IShowFileStorage _storage;
        public ExtendedFile CurrentShow { get; private set; }

        public List<ExtendedAxis> axes;

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
            CurrentShow = new ExtendedFile();
        }

    }
}
