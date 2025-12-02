using System;
using System.Linq;
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

        public async Task<string?> ShowSaveDialogAsync(string filter, string defaultExt, string initialDirectory)
        {
            var topLevel = GetTopLevel();
            if (topLevel == null) return null;

            var result = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Game",
                DefaultExtension = defaultExt,
                FileTypeChoices = new[] { new FilePickerFileType("Asteroid Save") { Patterns = new[] { "*.save" } } }
            });

            return result?.Path.LocalPath;
        }

        public async Task<string?> ShowOpenDialogAsync(string filter, string defaultExt, string initialDirectory)
        {
            var topLevel = GetTopLevel();
            if (topLevel == null) return null;

            var result = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Load Game",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("Asteroid Save") { Patterns = new[] { "*.save" } } }
            });

            return result.Count > 0 ? result[0].Path.LocalPath : null;
        }
    }
}