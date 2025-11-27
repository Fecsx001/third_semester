// C#
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;

namespace AsteroidGameAvalonia.ViewModels
{
    public sealed class AsyncDelegateCommand : ICommand
    {
        private readonly Func<object, Task> _executeAsync;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public AsyncDelegateCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public async void Execute(object parameter)
        {
            try
            {
                await _executeAsync(parameter).ConfigureAwait(false);
            }
            catch
            {
            }
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler == null) return;

            if (Dispatcher.UIThread.CheckAccess())
            {
                handler.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Dispatcher.UIThread.Post(() => handler.Invoke(this, EventArgs.Empty));
            }
        }
    }
}