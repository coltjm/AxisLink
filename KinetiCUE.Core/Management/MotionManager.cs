using KinetiCUE.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Management
{
    public class MotionManager
    {
        private Dictionary<string, IMotionService> _services;
        private readonly ShowFileManager _showFileManager;
        public MotionManager(List<string> configs, ShowFileManager showFileManager)
        {
            _showFileManager = showFileManager;
            // Constructor takes in a list of configs on creation from the file
        }

        
        public void Startup()
        {
            // Go through all controllers in the file and create the associated motion services
            
        }

        public void Refresh()
        {
            // Go through all controllers in the file and create/delete motion services to reflect changes
            
        }

        
        
    }
}
