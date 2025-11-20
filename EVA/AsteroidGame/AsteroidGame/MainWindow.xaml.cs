using System.Windows;
using AsteroidGame.View;
using AsteroidGame.ViewModel;

namespace AsteroidGame
{
    public partial class MainWindow : Window
    {
        private GameViewModel? _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => OnWindowLoaded();
        }

        private void OnWindowLoaded()
        {
            _viewModel = DataContext as GameViewModel;
            if (_viewModel != null)
            {
                int width = (int)GameArea.ActualWidth;
                int height = (int)GameArea.ActualHeight;
                _viewModel.SetSize(width, height);
            }
        }
    }
}