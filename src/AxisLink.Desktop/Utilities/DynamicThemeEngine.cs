using Avalonia;
using Avalonia.Media;
using AxisLink.Core.Models.Configs;
using System;

namespace AxisLink.Desktop.Utilities
{
    public static class DynamicThemeEngine
    {
        public static void ApplyCustomTheme(AppThemeConfig config)
        {
            if (Application.Current?.Resources == null) return;
            var resources = Application.Current.Resources;

            // --- PARSE CONFIG COLORS ---
            var accentColor = Color.Parse(config.AppAccent);
            var accentTextColor = Color.Parse(config.AppAccentText);
            var baseBgColor = Color.Parse(config.AppBackground);
            var surfaceColor = Color.Parse(config.AppSurface);
            var surfaceInputColor = Color.Parse(config.AppSurfaceInput);
            var borderColor = Color.Parse(config.AppBorder);
            var textColor = Color.Parse(config.AppText);
            var secondaryTextColor = Color.Parse(config.AppTextSecondary);

            // --- BRUSHES ---
            var accentBrush = new SolidColorBrush(accentColor);
            var accentTextBrush = new SolidColorBrush(accentTextColor);
            var bgBrush = new SolidColorBrush(baseBgColor);
            var surfaceBrush = new SolidColorBrush(surfaceColor);
            var inputBgBrush = new SolidColorBrush(surfaceInputColor);
            var borderBrush = new SolidColorBrush(borderColor);
            var textBrush = new SolidColorBrush(textColor);
            var secondaryTextBrush = new SolidColorBrush(secondaryTextColor);
            var menuBgBrush = new SolidColorBrush(Color.Parse(config.AppMenuBackground));

            // Status Colors
            var statusFaultColor = Color.Parse(config.StatusFault);
            var statusFaultBrush = new SolidColorBrush(statusFaultColor);

            // Adaptive interaction tints
            var subtleHoverBrush = new SolidColorBrush(Color.FromArgb(24, textColor.R, textColor.G, textColor.B));
            var buttonHoverBrush = new SolidColorBrush(Color.FromArgb(40, textColor.R, textColor.G, textColor.B));

            // AXISLINK CORE DESIGN TOKENS
            resources["AppAccent"] = accentBrush;
            resources["AppAccentText"] = accentTextBrush;
            resources["AppBackground"] = bgBrush;
            resources["AppSurface"] = surfaceBrush;
            resources["AppSurfaceInput"] = inputBgBrush;
            resources["AppBorder"] = borderBrush;
            resources["AppText"] = textBrush;
            resources["AppTextSecondary"] = secondaryTextBrush;
            resources["AppMenuBackground"] = menuBgBrush;

            // Machinery Status Badges
            resources["StatusEnabled"] = new SolidColorBrush(Color.Parse(config.StatusEnabled));
            resources["StatusFault"] = statusFaultBrush;
            resources["StatusWarning"] = new SolidColorBrush(Color.Parse(config.StatusWarning));
            resources["StatusMoving"] = new SolidColorBrush(Color.Parse(config.StatusMoving));

            // AVALONIA SYSTEM CONTROLS (TextBox, ComboBox, Lists)
            resources["SystemControlBackgroundAltHighBrush"] = bgBrush;
            resources["SystemControlBackgroundBaseLowBrush"] = surfaceBrush;
            resources["SystemControlForegroundBaseHighBrush"] = textBrush;
            resources["SystemControlForegroundBaseMediumBrush"] = secondaryTextBrush;
            resources["SystemControlHighlightAccentBrush"] = accentBrush;
            resources["SystemControlHighlightListLowBrush"] = new SolidColorBrush(Color.FromArgb(40, accentColor.R, accentColor.G, accentColor.B));

            // TextBoxes & Input Surfaces
            resources["TextControlBackground"] = inputBgBrush;
            resources["TextControlBackgroundPointerOver"] = inputBgBrush;
            resources["TextControlBackgroundFocused"] = inputBgBrush;
            resources["TextControlBorderBrush"] = borderBrush;
            resources["TextControlBorderBrushPointerOver"] = accentBrush;
            resources["TextControlBorderBrushFocused"] = accentBrush;
            resources["TextControlForeground"] = textBrush;
            resources["TextControlPlaceholderForeground"] = secondaryTextBrush;

            // MENUS & FLYOUTS
            resources["MenuForeground"] = textBrush;
            resources["MenuFlyoutItemForeground"] = textBrush;
            resources["MenuFlyoutItemForegroundPointerOver"] = textBrush;
            resources["MenuFlyoutItemForegroundSubMenuOpen"] = textBrush;
            resources["MenuFlyoutItemBackground"] = menuBgBrush;
            resources["MenuFlyoutItemBackgroundPointerOver"] = accentBrush;

            // DOCK.AVALONIA TOKENS
            resources["DockThemeBackgroundBrush"] = bgBrush;
            resources["DockThemeForegroundBrush"] = textBrush;
            resources["DockSurfaceWorkbenchBrush"] = bgBrush;
            resources["DockSurfaceSidebarBrush"] = surfaceBrush;
            resources["DockSurfaceEditorBrush"] = surfaceBrush;
            resources["DockSurfacePanelBrush"] = surfaceBrush;
            resources["DockSurfaceHeaderBrush"] = surfaceBrush;
            resources["DockSurfaceHeaderActiveBrush"] = surfaceBrush;

            // Structure & Borders
            resources["DockBorderSubtleBrush"] = borderBrush;
            resources["DockBorderStrongBrush"] = borderBrush;
            resources["DockSplitterIdleBrush"] = borderBrush;
            resources["DockSplitterHoverBrush"] = accentBrush;
            resources["DockSplitterDragBrush"] = accentBrush;
            resources["DockTargetIndicatorBrush"] = accentBrush;

            // Chrome / Close Button Icons & Hover
            resources["DockToolChromeIconBrush"] = secondaryTextBrush;
            resources["DockChromeButtonForegroundBrush"] = secondaryTextBrush;
            resources["DockChromeButtonHoverBackgroundBrush"] = buttonHoverBrush;
            resources["DockChromeButtonDangerHoverBrush"] = statusFaultBrush;

            // Unselected tab strip background (flat & transparent)
            resources["DockTabBackgroundBrush"] = new SolidColorBrush(Colors.Transparent);
            resources["DockDocumentTabStripBackgroundBrush"] = new SolidColorBrush(Colors.Transparent);
            resources["DockTabForegroundBrush"] = secondaryTextBrush;
            resources["DockTabHoverBackgroundBrush"] = subtleHoverBrush;

            // Active tab background & text
            resources["DockTabActiveBackgroundBrush"] = surfaceBrush;
            resources["DockTabActiveForegroundBrush"] = textBrush;
            resources["DockTabSelectedForegroundBrush"] = textBrush;

            // Tab Active Indicator Accent Line
            resources["DockTabActiveIndicatorBrush"] = accentBrush;

            // SYSTEM CONSOLE / LOGGING
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