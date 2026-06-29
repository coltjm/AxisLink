using KinetiCUE.Core.Models.KQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Interfaces
{
    public interface IShowFileStorage
    {
        //holds all motion services and pollers
        public KQFile LoadFromFileSystem(string path);

        public void SaveToFileSystem(KQFile show, string path);

        public void SaveAsToFileSystem(KQFile show, string path);
    }
}
