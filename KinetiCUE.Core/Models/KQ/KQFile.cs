using KinetiCUE.Core.Models.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Models.KQ
{
    public class KQFile : StandardFile
    {
        public KQFile(List<StandardAxis> Axes, List<StandardGroup> Groups, List<StandardScenery> Scenery, List<StandardPatch> Patches, List<StandardCue> Cues) 
            : base(Axes, Groups, Scenery, Patches,  Cues)
        {
        }
    }
}
