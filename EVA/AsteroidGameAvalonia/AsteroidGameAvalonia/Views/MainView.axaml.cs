using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AsteroidGameAvalonia.ViewModels;
using AsteroidGameAvalonia.Services;

namespace AsteroidGameAvalonia.Views
{
    public partial class MainView : UserControl
    {
        private MainViewModel? _viewModel;

        public MainView()
        {
            InitializeComponent();
            this.AttachedToVisualTree += (s, e) => this.Focus();
            
            AvaloniaDialogService.RequestMessageOverlay += ShowOverlay;
            AvaloniaDialogService.RequestConfirmationOverlay += ShowConfirmationOverlay;
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            _viewModel = DataContext as MainViewModel;
            if (_viewModel != null)
            {
                _viewModel.SetSize(800, 600);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_viewModel == null) return;
            switch (e.Key)
            {
                case Key.Left: _viewModel.SetMovingLeft(true); break;
                case Key.Right: _viewModel.SetMovingRight(true); break;
                case Key.Space:
                    if (_viewModel.IsGameOver) _viewModel.StartNewGame();
                    else _viewModel.TogglePause();
                    break;
                case Key.Escape: _viewModel.StopGame(); break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (_viewModel == null) return;
            switch (e.Key)
            {
                case Key.Left: _viewModel.SetMovingLeft(false); break;
                case Key.Right: _viewModel.SetMovingRight(false); break;
            }
            base.OnKeyUp(e);
        }

        private void OnLeftPressed(object? sender, PointerPressedEventArgs e) => _viewModel?.SetMovingLeft(true);
        private void OnLeftReleased(object? sender, PointerReleasedEventArgs e) => _viewModel?.SetMovingLeft(false);
        private void OnRightPressed(object? sender, PointerPressedEventArgs e) => _viewModel?.SetMovingRight(true);
        private void OnRightReleased(object? sender, PointerReleasedEventArgs e) => _viewModel?.SetMovingRight(false);

        private void ShowOverlay(string message, string title)
        {
            Dispatcher.UIThread.Invoke(() => 
            {
                MessageTitle.Text = title;
                MessageText.Text = message;
                MessageOverlay.IsVisible = true;
            });
        }

        private Task<bool> ShowConfirmationOverlay(string message, string title)
        {
            ShowOverlay(message, title); 
            return Task.FromResult(true); 
        }

        private void CloseMessageOverlay(object? sender, RoutedEventArgs e)
        {
            MessageOverlay.IsVisible = false;
            this.Focus();
        }
    }
}