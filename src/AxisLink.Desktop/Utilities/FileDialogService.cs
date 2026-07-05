using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AxisLink.Core.Interfaces;
using AxisLink.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AxisLink.Desktop.Utilities
{
    public class FileDialogService
    {
        public async Task<string?> OpenFileDialogAsync(string title, string[] extensions, string filterName)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return null;
            }

            // Use the active window or fall back to the main window
            Window? targetWindow = desktop.MainWindow;
            if (targetWindow == null) return null;

            var options = new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                new FilePickerFileType(filterName)
                {
                    Patterns = new List<string>(extensions)
                }
            }
            };
            var result = await targetWindow.StorageProvider.OpenFilePickerAsync(options);

            return result.Count > 0 ? result[0].Path.LocalPath : null;
        }

        // allow custom extensions if saving something other than show file
        public async Task<string[]?> SaveFileDialogAsync(string title, FilePickerFileType[] extensionsChoices)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return null;
            }

            // Use the active window or fall back to the main window
            Window? targetWindow = desktop.MainWindow;
            if (targetWindow == null) return null;

            var options = new FilePickerSaveOptions
            {
                Title = title,
                FileTypeChoices = extensionsChoices
            };
            var result = await targetWindow.StorageProvider.SaveFilePickerWithResultAsync(options);
            var format = result.SelectedFileType?.Name; 
            return [result.File?.Path.LocalPath, format];
        }
        
        // Default overload to axislink file saving
        public async Task<string[]?> SaveFileDialogAsync(string title, string showName)
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            {
                return null;
            }

            // Use the active window or fall back to the main window
            Window? targetWindow = desktop.MainWindow;
            if (targetWindow == null) return null;

            var options = new FilePickerSaveOptions
            {
                Title = title,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("AxisLink Project")
                    {
                        Patterns = new[] { "*.alink" }
                    },
                    new FilePickerFileType("ESTA XML File")
                    {
                        Patterns = new[] { "*.xml" }
                    }
                },
                SuggestedFileName = showName,
                SuggestedFileType = new FilePickerFileType("AxisLink Project")
                {
                    Patterns = new[] { "*.alink" }
                }
            };
            var result = await targetWindow.StorageProvider.SaveFilePickerWithResultAsync(options);
            var format = result.SelectedFileType?.Name; // AxisLink Project or ESTA XML File to save appropriately
            return [result.File?.Path.LocalPath, format];
        }
    }
}
