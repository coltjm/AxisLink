using Avalonia;
using Avalonia.Media;
using KinetiCUE.Core.Models.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace KinetiCUE.Desktop.Utilities
{
    public static class DynamicThemeEngine
    {
        public static void ApplyCustomTheme(KQThemeConfig config)
        {
            var resources = Application.Current!.Resources;

            var accentBrush = new SolidColorBrush(Color.Parse(config.KQAccent));
            var bgBrush = new SolidColorBrush(Color.Parse(config.KQBackground));
            var textBrush = new SolidColorBrush(Color.Parse(config.KQText));

            //----------------KQ APP-------------------------------
            resources["KQAccent"] = accentBrush;
            resources["KQBackground"] = bgBrush;
            resources["KQText"] = textBrush;
            resources["KQMenuBackground"] = new SolidColorBrush(Color.Parse(config.KQMenuBackground));

            // --- GLOBAL WINDOW & PANEL BASES ---
            resources["SystemControlBackgroundAltHighBrush"] = bgBrush; // Window Backgrounds
            resources["SystemControlBackgroundBaseLowBrush"] = new SolidColorBrush(Color.Parse(config.ConsoleBackground)); // Inner panels / TextBoxes

            // --- GLOBAL TEXT / FOREGROUNDS ---
            resources["SystemControlForegroundBaseHighBrush"] = textBrush; // Primary Labels & Content
            resources["SystemControlForegroundBaseMediumBrush"] = textBrush; // Subheaders / Watermarks

            // --- GLOBAL ACCENTS & INTERACTIONS ---
            resources["SystemControlHighlightAccentBrush"] = accentBrush; // Checked States / Sliders
            resources["SystemControlHighlightListLowBrush"] = accentBrush; // List Selection Hover
            // MENU ITEMS
            resources["MenuForeground"] = textBrush;

                // Dropdown Menu Item Text Colors
            resources["MenuFlyoutItemForeground"] = textBrush;
            resources["MenuFlyoutItemForegroundPointerOver"] = textBrush;
            resources["MenuFlyoutItemForegroundSubMenuOpen"] = textBrush;

                // Dropdown Menu Item Backgrounds
            resources["MenuFlyoutItemBackground"] = new SolidColorBrush(Color.Parse(config.KQMenuBackground));
            resources["MenuFlyoutItemBackgroundPointerOver"] = accentBrush;
            // DOCK
            resources["DockApplicationAccentBrushLow"] = accentBrush;
            resources["DockApplicationAccentBrushMed"] = accentBrush;
            resources["DockApplicationAccentBrushHigh"] = accentBrush;
            resources["DockApplicationAccentForegroundBrush"] = textBrush;
            resources["DockApplicationAccentBrushIndicator"] = accentBrush;

            resources["DockThemeBackgroundBrush"] = bgBrush;
            resources["DockThemeForegroundBrush"] = textBrush;
            resources["DockThemeControlBackgroundBrush"] = bgBrush;

            resources["DockSurfaceWorkbenchBrush"] = bgBrush;
            resources["DockSurfaceSidebarBrush"] = bgBrush;
            resources["DockSurfaceEditorBrush"] = bgBrush;
            resources["DockSurfacePanelBrush"] = bgBrush;

                    // Tab Headers & Chrome Selections
            resources["DockTabActiveBackgroundBrush"] = accentBrush;
            resources["DockTabActiveIndicatorBrush"] = accentBrush;
            resources["DockTabSelectedForegroundBrush"] = accentBrush;
            resources["DockTabActiveForegroundBrush"] = textBrush;
            resources["DockDocumentTabSelectedForegroundBrush"] = textBrush;

                    // Splitters and Target Blue Highlight Boxes
            resources["DockSplitterHoverBrush"] = accentBrush;
            resources["DockSplitterDragBrush"] = accentBrush;
            resources["DockTargetIndicatorBrush"] = accentBrush;
            //-----------------CONSOLE-------------------------------
            resources["ConsoleBackground"] = new SolidColorBrush(Color.Parse(config.ConsoleBackground));
            resources["ConsoleHeaderBackground"] = new SolidColorBrush(Color.Parse(config.ConsoleHeaderBackground));
            resources["ConsoleText"] = new SolidColorBrush(Color.Parse(config.ConsoleText));
            resources["ConsoleHeaderText"] = new SolidColorBrush(Color.Parse(config.ConsoleHeaderText));
            resources["ConsoleWarningText"] = new SolidColorBrush(Color.Parse(config.ConsoleWarningText));
            resources["ConsoleErrorText"] = new SolidColorBrush(Color.Parse(config.ConsoleErrorText));
            resources["ConsoleCriticalText"] = new SolidColorBrush(Color.Parse(config.ConsoleCriticalText));
        }
    }
}
