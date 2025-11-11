using System.Configuration;
using System.Data;
using System.Windows;
using PingPongGame.ViewModel;

namespace PingPongGame;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private MainWindow _window;
    private MainViewModel _mainViewModel;
    
    
    public App()
    {
        Startup += App_Startup;   
    }
    
    private void App_Startup(object sender, StartupEventArgs e)
    {
        _mainViewModel = new MainViewModel();
        _mainViewModel.PadNextPosition += OnPadNextPosition; 
        _mainViewModel.BallNextPosition += OnBallNextPosition;
        _mainViewModel.GameOver += OnGameOver;
        _window = new MainWindow()
        {
            DataContext = _mainViewModel
        };
        
        
        _window.BallLayoutUpdated += OnBallLayoutUpdated;
        _window.PadLayoutUpdated += OnPadLayoutUpdated;
        
        _window.Show();
    }

    private void OnBallLayoutUpdated(object sender, Thickness e)
    {
        _mainViewModel.MoveBall(e.Left, e.Top);
    }
    private void OnPadLayoutUpdated(object sender, Thickness e)
    {
        _mainViewModel.MovePad(e.Left, e.Top);
    }
    
    private void OnPadNextPosition(object? sender, Thickness e)
    {
        _window.StartPadAnimation(e);
    }
    
    private void OnBallNextPosition(object? sender, Thickness e)
    {
        _window.StartBallAnimation(e);
    }
    
    private void OnGameOver(object? sender, EventArgs e)
    {
        var result = MessageBox.Show("Game Over!");
        if (result == MessageBoxResult.OK)
        {
            Current.Shutdown();
        }
    }
}