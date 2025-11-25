using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AsteroidGameAvalonia.ViewModels; 

namespace AsteroidGameAvalonia.Services
{
    public class AvaloniaDialogService : IDialogService
    {
        public static event Action<string, string>? RequestMessageOverlay;
        public static event Func<string, string, Task<bool>>? RequestConfirmationOverlay;

        private TopLevel? GetTopLevel()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return TopLevel.GetTopLevel(desktop.MainWindow);
            }
            else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                return TopLevel.GetTopLevel(singleView.MainView);
            }
            return null;
        }

        public void ShowMessage(string message, string title)
        {
            RequestMessageOverlay?.Invoke(message, title);
        }

        public bool ShowConfirmation(string message, string title)
        {
            var task = RequestConfirmationOverlay?.Invoke(message, title);
            if (task != null)
            {
                task.Wait(); 
                return task.Result;
            }
            return true;
        }

        public string? ShowSaveDialog(string filter, string defaultExt, string initialDirectory)
        {
            var topLevel = GetTopLevel();
            if (topLevel == null) return null;

            var task = topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Game",
                DefaultExtension = defaultExt,
                FileTypeChoices = new[] { new FilePickerFileType("Asteroid Save") { Patterns = new[] { "*.save" } } }
            });

            task.Wait(); 
            return task.Result?.Path.LocalPath;
        }

        public string? ShowOpenDialog(string filter, string defaultExt, string initialDirectory)
        {
            var topLevel = GetTopLevel();
            if (topLevel == null) return null;

            var task = topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Load Game",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("Asteroid Save") { Patterns = new[] { "*.save" } } }
            });

            task.Wait();
            return task.Result.Count > 0 ? task.Result[0].Path.LocalPath : null;
        }
    }
}