using AxisLink.Core.Models.Sprockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisLink.Core.Interfaces
{
    public interface ISprocketProvider
    {
        // Gets all available sprockets (built-ins, local user library, and show-embedded).
        IEnumerable<Sprocket> GetAvailableSprockets();

        // Finds a sprocket by its ID.
        Sprocket? GetSprocketById(string sprocketId);
    }
}
