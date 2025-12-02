using System.Threading.Tasks;

namespace AsteroidGameAvalonia.ViewModels
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool ShowConfirmation(string message, string title);
        Task<string?> ShowSaveDialogAsync(string filter, string defaultExt, string initialDirectory);
        Task<string?> ShowOpenDialogAsync(string filter, string defaultExt, string initialDirectory);
    }
}