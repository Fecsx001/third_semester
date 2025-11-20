using System.Windows;
using Microsoft.Win32;
using AsteroidGame.ViewModel;

namespace AsteroidGame.View
{
    public class WpfDialogService : IDialogService
    {
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ShowConfirmation(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public string? ShowSaveDialog(string filter, string defaultExt, string initialDirectory)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = "Save Game",
                InitialDirectory = initialDirectory
            };
            
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string? ShowOpenDialog(string filter, string defaultExt, string initialDirectory)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = "Load Game",
                InitialDirectory = initialDirectory,
                CheckFileExists = true
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}