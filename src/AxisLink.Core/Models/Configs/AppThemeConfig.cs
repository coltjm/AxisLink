using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AxisLink.Core.Models.Configs
{
    public class AppThemeConfig
    {
        // --------------------------------------------------------------------
        // METADATA
        // --------------------------------------------------------------------
        public string ThemeName { get; set; } = "Dark";
        public bool IsCustom { get; set; } = false;

        // --------------------------------------------------------------------
        // PRIMARY WORKSPACE CHROME & SURFACES
        // --------------------------------------------------------------------
        // Base canvas/window background (lowest layer)
        public string AppBackground { get; set; } = "#18181B";

        // Lifted panels, dock surfaces, and card containers
        public string AppSurface { get; set; } = "#27272A";

        // Sunken/recessed areas: TextBoxes, ComboBoxes, Numeric inputs
        public string AppSurfaceInput { get; set; } = "#121214";

        // Subtle container borders and splitters
        public string AppBorder { get; set; } = "#3F3F46";

        // Menu bar and floating flyouts
        public string AppMenuBackground { get; set; } = "#202024";

        // --------------------------------------------------------------------
        // ACCENTS & HIGHLIGHTS
        // --------------------------------------------------------------------
        // Primary brand/interaction color (buttons, active tabs, focus rings)
        public string AppAccent { get; set; } = "#7C3AED";

        // High-luminance accent specifically for headers and labels on dark cards
        public string AppAccentText { get; set; } = "#A78BFA";

        // --------------------------------------------------------------------
        // TYPOGRAPHY / FOREGROUNDS
        // --------------------------------------------------------------------
        // Primary high-contrast text (values, headings, main labels)
        public string AppText { get; set; } = "#F4F4F5";

        // Muted secondary text (field labels, units, placeholders, trims)
        public string AppTextSecondary { get; set; } = "#A1A1AA";

        // --------------------------------------------------------------------
        // MACHINERY STATUS & TELEMETRY (ANSI E1.44 / INDUSTRIAL)
        // --------------------------------------------------------------------
        public string StatusEnabled { get; set; } = "#22C55E";   // Drive ready / enabled
        public string StatusFault { get; set; } = "#EF4444";     // Emergency stop / drive fault
        public string StatusWarning { get; set; } = "#F59E0B";   // Near soft limit / warning
        public string StatusMoving { get; set; } = "#0EA5E9";    // Active motion execution

        // --------------------------------------------------------------------
        // CONSOLE & LOGGING
        // --------------------------------------------------------------------
        public string ConsoleBackground { get; set; } = "#121214";
        public string ConsoleHeaderBackground { get; set; } = "#202024";
        public string ConsoleText { get; set; } = "#38BDF8";
        public string ConsoleHeaderText { get; set; } = "#94A3B8";
        public string ConsoleWarningText { get; set; } = "#FBBF24";
        public string ConsoleErrorText { get; set; } = "#F87171";
        public string ConsoleCriticalText { get; set; } = "#F43F5E";

        // --------------------------------------------------------------------
        // FACTORY PRESETS
        // --------------------------------------------------------------------
        public static AppThemeConfig DefaultDark() => new()
        {
            ThemeName = "Default Dark",
            AppBackground = "#18181B",
            AppSurface = "#27272A",
            AppSurfaceInput = "#121214",
            AppBorder = "#3F3F46",
            AppAccent = "#7C3AED",
            AppAccentText = "#A78BFA",
            AppText = "#F4F4F5",
            AppTextSecondary = "#A1A1AA"
        };

        public static AppThemeConfig ModernSlate() => new()
        {
            ThemeName = "Modern Slate",
            AppBackground = "#0F172A",
            AppSurface = "#1E293B",
            AppSurfaceInput = "#0B1120",
            AppBorder = "#334155",
            AppAccent = "#0284C7",
            AppAccentText = "#38BDF8",
            AppText = "#F8FAFC",
            AppTextSecondary = "#94A3B8"
        };

        public static AppThemeConfig HighContrastBooth() => new()
        {
            ThemeName = "High-Contrast Booth",
            AppBackground = "#000000",
            AppSurface = "#141414",
            AppSurfaceInput = "#050505",
            AppBorder = "#404040",
            AppAccent = "#DC2626",
            AppAccentText = "#F87171",
            AppText = "#FFFFFF",
            AppTextSecondary = "#D4D4D4"
        };
    }
}