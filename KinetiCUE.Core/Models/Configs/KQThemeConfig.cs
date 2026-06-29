using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Core.Models.Configs
{
    public class KQThemeConfig
    {
        // NAME
        public string ThemeName { get; set; } = "Dark";
        // FULL APP THEME COLORS
        public string KQAccent { get; set; } = "#46169e";
        public string KQBackground { get; set; } = "#1e1e1e";
        public string KQText { get; set; } = "#ffffff";
        public string KQMenuBackground { get; set; } = "#2d2d2d";
        // CONSOLE THEME COLORS
        public string ConsoleBackground { get; set; } = "#1e1e1e";
        public string ConsoleHeaderBackground { get; set; } = "#2d2d2d";
        public string ConsoleText { get; set; } = "#00ff00";
        public string ConsoleHeaderText { get; set; } = "#aaaaaa";
        public string ConsoleWarningText { get; set; } = "#ffff00";
        public string ConsoleErrorText { get; set; } = "#ff0000";
        public string ConsoleCriticalText { get; set; } = "#ff00ff";

        // AXIS VIEWER THEME COLORS
    }
}
