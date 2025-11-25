using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AsteroidGameAvalonia.ViewModels; 
using AsteroidGameAvalonia.Persistance;
using AsteroidGameAvalonia.Services;
using AsteroidGameAvalonia.Views;
using System;

namespace AsteroidGameAvalonia
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            string appPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            
            IHighScoreManager highScoreManager = new HighScoreManager(appPath);
            IGamePersistence persistence = new GamePersistence();
            IDialogService dialogService = new AvaloniaDialogService();

            var viewModel = new MainViewModel(persistence, highScoreManager, dialogService);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = viewModel
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleView)
            {
                singleView.MainView = new MainView
                {
                    DataContext = viewModel
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}