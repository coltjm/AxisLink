using AxisLink.Core.Models.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    public interface IShowFileStorage
    {
        //holds all motion services and pollers
        public ExtendedFile LoadFromFileSystem(string path);

        public void SaveToFileSystem(ExtendedFile show, string path);

        public void SaveAsToFileSystem(ExtendedFile show, string path);
    }
}
