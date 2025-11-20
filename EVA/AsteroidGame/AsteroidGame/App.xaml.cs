using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using AsteroidGame.View;
using AsteroidGame.ViewModel;
using AsteroidGameMechanic.Persistance;

namespace AsteroidGame
{
    public partial class App : IDisposable
    {
        private MainWindow? _window = null;
        private GameViewModel _viewModel = null!;
        private bool _disposed = false;
        
        public App()
        {
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? "";
            IHighScoreManager highScoreManager = new HighScoreManager(appPath);
            IGamePersistence persistence = new GamePersistence();
            
            IDialogService dialogService = new WpfDialogService();

            _viewModel = new GameViewModel(persistence, highScoreManager, dialogService);
            
            _viewModel.GameOver += OnGameOver;
            _viewModel.GameLoaded += OnGameLoaded;
            
            var window = new MainWindow
            {
                DataContext = _viewModel
            };
            
            window.KeyDown += OnWindowKeyDown;
            window.KeyUp += OnWindowKeyUp;
            window.Closing += OnWindowClosing;
            
            _window = window;             
            window.Show();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        { 
            if (_viewModel == null) return;

            switch (e.Key)
            {
                case Key.Left:
                    _viewModel.SetMovingLeft(true);
                    break;
                case Key.Right:
                    _viewModel.SetMovingRight(true);
                    break;
                case Key.Space:
                    if (_viewModel.IsGameOver)
                    {
                        _viewModel.StartNewGame();
                    }
                    else
                    {
                        _viewModel.TogglePause();
                    }
                    break;
                case Key.Escape:
                    _window?.Close();
                    break;
            }
        }
        
        private void OnWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (_viewModel == null) return;

            switch (e.Key)
            {
                case Key.Left:
                    _viewModel.SetMovingLeft(false);
                    break;
                case Key.Right:
                    _viewModel.SetMovingRight(false);
                    break;
            }
        }
        
        private void OnWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel?.StopGame();
        }
        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_viewModel is IDisposable disposableViewModel)
                { 
                    _viewModel.GameOver -= OnGameOver;
                    _viewModel.GameLoaded -= OnGameLoaded;
                    disposableViewModel.Dispose();
                }
                else if (_viewModel != null)
                {
                    _viewModel.GameOver -= OnGameOver;
                    _viewModel.GameLoaded -= OnGameLoaded;
                }
            }
            catch
            {
                // ignore
            }

            if (_window is not null)
            {
                _window.KeyDown -= OnWindowKeyDown;
                _window.KeyUp -= OnWindowKeyUp;
                _window.Closing -= OnWindowClosing;
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }


        private void OnGameOver(object? sender, EventArgs e)
        {
            if (_viewModel == null) return;

            var vm = _viewModel;
            string message = $"Game Over!\nTime: {vm.GameTime:mm\\:ss}\nScore: {vm.Score}";
            if (vm.Score == vm.HighScore && vm.Score > 0)
            {
                message += "\n🎉 NEW HIGH SCORE! 🎉";
            }
            MessageBox.Show(message, "Game Over");
        }
        
        private void OnGameLoaded(object? sender, EventArgs e)
        {
            _window?.Focus();
        }
    }
}