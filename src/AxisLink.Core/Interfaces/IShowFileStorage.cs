using AxisLink.Core.Models.Show;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    public interface IShowFileStorage
    {
        //holds all motion services and pollers
        public ShowFile LoadFromFileSystem(string path);

        public void SaveToFileSystem(ShowFile show, string path);

        public void SaveAsToFileSystem(ShowFile show, string path);
    }
}
