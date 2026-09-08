using AxisLink.Core.Interfaces;
using AxisLink.Core.Management;
using AxisLink.Core.Models.Show;
using AxisLink.Core.Models.Sprockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AxisLink.Infrastructure.Sprockets
{
    public class SprocketProvider : ISprocketProvider
    {
        private readonly ShowFileManager _fileManager;
        private readonly string _userFolder;

        public SprocketProvider(ShowFileManager fileManager)
        {
            _fileManager = fileManager;
            _userFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AxisLink",
                "Sprockets"
            );
            Directory.CreateDirectory(_userFolder);
        }

        public IEnumerable<Sprocket> GetAvailableSprockets()
        {
            var dict = new Dictionary<string, Sprocket>();

            //Add compiled built-in sprockets
            dict[BuiltInSprockets.ClickPlcStepper.Id] = BuiltInSprockets.ClickPlcStepper;

            // TODO: Add local disk files from user folder (when JSON reader is ready)

            // Add Show-embedded sprockets (takes precedence)
            var embeddedPayloads = _fileManager.CurrentShow?.EmbeddedSprockets;
            if (embeddedPayloads != null)
            {
                foreach (var json in embeddedPayloads)
                {
                    if (string.IsNullOrWhiteSpace(json)) continue;

                    try
                    {
                        var s = JsonSerializer.Deserialize<Sprocket>(json);
                        if (s != null && !string.IsNullOrEmpty(s.Id))
                        {
                            dict[s.Id] = s; // Embedded overrides built-in if IDs collide
                        }
                    }
                    catch
                    {
                        // Safely skip malformed JSON entries
                    }
                }
            }

                return dict.Values;
        }

        public Sprocket? GetSprocketById(string id)
        {
            return GetAvailableSprockets().FirstOrDefault(s => s.Id == id);
        }
    }
}
