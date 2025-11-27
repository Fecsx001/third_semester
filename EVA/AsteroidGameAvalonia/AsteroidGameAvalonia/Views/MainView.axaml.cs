using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        
        // Smoothing for touch movement
        private readonly Queue<(DateTime timestamp, int targetX)> _targetPositionHistory = new();
        private const double SmoothingWindowMs = 150; //ms
        private const int DeadZonePixels = 50;

        public MainView()
        {
            InitializeComponent();
            this.AttachedToVisualTree += (_, _) => this.Focus();
            
            this.SizeChanged += OnSizeChanged;
            
            AvaloniaDialogService.RequestMessageOverlay += ShowOverlay;
            AvaloniaDialogService.RequestConfirmationOverlay += ShowConfirmationOverlay;
        }
        
        private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            if (_viewModel != null && e.NewSize.Width > 0 && e.NewSize.Height > 0)
            {
                bool isAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));
                if (isAndroid)
                {
                    int spawnYOffset = 120;
                    _viewModel.SetSize((int)e.NewSize.Width, (int)e.NewSize.Height, spawnYOffset);
                }
            }
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            base.OnDataContextChanged(e);
            _viewModel = DataContext as MainViewModel;
            if (_viewModel != null)
            {
                _viewModel.GameOver += OnGameOver;
                
                bool isAndroid = RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));

                if (isAndroid)
                {
                    void ApplyAndroidSize()
                    {
                        double w = this.Bounds.Width;
                        double h = this.Bounds.Height;
                        if (w <= 0 || h <= 0) return;

                        int spawnYOffset = 120;
                        _viewModel.SetSize((int)w, (int)h, spawnYOffset);
                    }

                    if (this.Bounds.Width > 0 && this.Bounds.Height > 0)
                    {
                        ApplyAndroidSize();
                    }
                    else
                    {
                        EventHandler? layoutHandler = null;
                        layoutHandler = (_, _) =>
                        {
                            if (this.Bounds.Width > 0 && this.Bounds.Height > 0)
                            {
                                ApplyAndroidSize();
                                if (layoutHandler != null)
                                    this.LayoutUpdated -= layoutHandler;
                            }
                        };
                        this.LayoutUpdated += layoutHandler;
                    }
                }
                else
                {
                    _viewModel.SetSize(800, 600);
                }
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

        private void OnGameAreaPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_viewModel == null) return;
            
            var point = e.GetPosition(GameArea);
            double touchX = point.X;
            
            int spaceshipWidth = _viewModel.Spaceship.Width;
            int targetX = (int)(touchX - spaceshipWidth / 2.0);
            
            int gameWidth = _viewModel.ScreenWidth;
            targetX = Math.Max(0, Math.Min(targetX, gameWidth - spaceshipWidth));
            
            DateTime now = DateTime.Now;
            _targetPositionHistory.Enqueue((now, targetX));
            
            while (_targetPositionHistory.Count > 0)
            {
                var oldest = _targetPositionHistory.Peek();
                if ((now - oldest.timestamp).TotalMilliseconds > SmoothingWindowMs)
                {
                    _targetPositionHistory.Dequeue();
                }
                else
                {
                    break;
                }
            }
            
            int smoothedTargetX = targetX;
            if (_targetPositionHistory.Count > 1)
            {
                var positions = new List<int>();
                foreach (var (_, x) in _targetPositionHistory)
                {
                    positions.Add(x);
                }
                
                positions.Sort();
                int median = positions[positions.Count / 2];
                
                int sum = 0;
                int count = 0;
                foreach (var pos in positions)
                {
                    if (Math.Abs(pos - median) <= 40)
                    {
                        sum += pos;
                        count++;
                    }
                }
                
                if (count > 0)
                {
                    smoothedTargetX = sum / count;
                }
                else
                {
                    smoothedTargetX = targetX;
                }
            }
            
            int currentX = _viewModel.Spaceship.X;
            int difference = Math.Abs(smoothedTargetX - currentX);
            
            if (difference > DeadZonePixels)
            {
                if (smoothedTargetX < currentX)
                {
                    _viewModel.SetMovingLeft(true);
                    _viewModel.SetMovingRight(false);
                }
                else
                {
                    _viewModel.SetMovingRight(true);
                    _viewModel.SetMovingLeft(false);
                }
            }
            else
            {
                _viewModel.SetMovingLeft(false);
                _viewModel.SetMovingRight(false);
            }
        }
        
        private void OnGameAreaPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (_viewModel == null) return;
            e.Handled = true;
        }
        
        private void OnGameAreaPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (_viewModel == null) return;
            
            _viewModel.SetMovingLeft(false);
            _viewModel.SetMovingRight(false);
            
            _targetPositionHistory.Clear();
            
            e.Handled = true;
        }

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
        
        private void OnGameOver(object? sender, EventArgs e)
        {
            if (_viewModel == null) return;
            
            string stats = $"Final Score: {_viewModel.Score}\n" +
                          $"Time Survived: {_viewModel.GameTime:mm\\:ss}\n" +
                          $"Difficulty: {_viewModel.Difficulty}\n" +
                          $"High Score: {_viewModel.HighScore}";
            
            ShowOverlay(stats, "GAME OVER - STATISTICS");
        }
    }
}