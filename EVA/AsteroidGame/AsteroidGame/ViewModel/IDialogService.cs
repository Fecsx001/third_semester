namespace AsteroidGame.ViewModel
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool ShowConfirmation(string message, string title);
        string? ShowSaveDialog(string filter, string defaultExt, string initialDirectory);
        string? ShowOpenDialog(string filter, string defaultExt, string initialDirectory);
    }
}
