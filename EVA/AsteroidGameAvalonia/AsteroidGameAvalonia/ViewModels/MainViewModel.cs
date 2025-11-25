using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input; 
using Avalonia; 
using Avalonia.Controls.ApplicationLifetimes; 
using Avalonia.Threading; 
using AsteroidGameAvalonia.Model;
using AsteroidGameAvalonia.Persistance;

namespace AsteroidGameAvalonia.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private GameModel _gameModel;
        private readonly IGamePersistence _persistence;
        private readonly IHighScoreManager _highScoreManager;
        private readonly IDialogService _dialogService;

        private Thickness _spaceshipPosition;

        public ObservableCollection<Asteroid> Asteroids { get; }
        
        public Thickness SpaceshipPosition
        {
            get => _spaceshipPosition;
            set
            {
                _spaceshipPosition = value;
                OnPropertyChanged();
            }
        }

        public Spaceship Spaceship => _gameModel.Spaceship;
        public int Score => _gameModel.Score;
        public TimeSpan GameTime => _gameModel.GameTime;
        public int HighScore => _gameModel.HighScore;
        public bool IsGameOver => _gameModel.IsGameOver;
        public bool IsPaused => _gameModel.IsPaused;
        public string Difficulty => _gameModel.GetDifficultyDescription();
        public int ScreenWidth => _gameModel.ScreenWidth;
        public int ScreenHeight => _gameModel.ScreenHeight;

        public ICommand NewGameCommand { get; }
        public ICommand TogglePauseCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand ResetHighScoreCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand ShowControlsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public event EventHandler? GameOver;
        public event EventHandler? GameLoaded;

        public MainViewModel(IGamePersistence persistence, IHighScoreManager highScoreManager, IDialogService dialogService)
        {
            _persistence = persistence;
            _highScoreManager = highScoreManager;
            _dialogService = dialogService;
            
            _gameModel = new GameModel(800, 600, _highScoreManager, _persistence);
            Asteroids = new ObservableCollection<Asteroid>();

            _gameModel.GameOver += OnGameOver;
            _gameModel.ScoreChanged += OnModelUpdate;
            _gameModel.GameTimeChanged += OnModelUpdate;
            _gameModel.HighScoreChanged += OnModelUpdate;

            NewGameCommand = new DelegateCommand(_ => ExecuteNewGame());
            TogglePauseCommand = new DelegateCommand(_ => ExecuteTogglePause());
            SaveGameCommand = new AsyncDelegateCommand(async _ => await ExecuteSaveGameAsync(), _ => _gameModel.IsPaused && !_gameModel.IsGameOver);
            LoadGameCommand = new AsyncDelegateCommand(async _ => await ExecuteLoadGameAsync());
            ResetHighScoreCommand = new DelegateCommand(_ => ExecuteResetHighScore());
            
            ExitCommand = new DelegateCommand(_ => 
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
                else
                {
                    System.Environment.Exit(0);
                }
            });

            ShowControlsCommand = new DelegateCommand(_ => ExecuteShowControls());
            ShowAboutCommand = new DelegateCommand(_ => ExecuteShowAbout());

            _gameModel.StartGame();
            UpdateSpaceshipPosition();
        }

        public void SetSize(int width, int height)
        {
            _gameModel.Stop();
            _gameModel = new GameModel(width, height, _highScoreManager, _persistence);

            _gameModel.GameOver += OnGameOver;
            _gameModel.ScoreChanged += OnModelUpdate;
            _gameModel.GameTimeChanged += OnModelUpdate;
            _gameModel.HighScoreChanged += OnModelUpdate;

            _gameModel.StartGame();

            OnPropertyChanged(nameof(ScreenWidth));
            OnPropertyChanged(nameof(ScreenHeight));
            OnModelUpdate(this, EventArgs.Empty);
        }

        private void UpdateSpaceshipPosition()
        {
            SpaceshipPosition = new Thickness(_gameModel.Spaceship.X, _gameModel.Spaceship.Y, 0, 0);
        }
        
        private void OnModelUpdate(object? sender, EventArgs e)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                OnPropertyChanged(nameof(Score));
                OnPropertyChanged(nameof(GameTime));
                OnPropertyChanged(nameof(HighScore));
                OnPropertyChanged(nameof(IsGameOver));
                OnPropertyChanged(nameof(IsPaused));
                OnPropertyChanged(nameof(Difficulty));
                
                UpdateSpaceshipPosition();
                
                Asteroids.Clear();
                foreach (var asteroid in _gameModel.Asteroids)
                {
                    Asteroids.Add(asteroid);
                }
                // Raise CanExecute on either DelegateCommand or AsyncDelegateCommand
                (SaveGameCommand as dynamic)?.RaiseCanExecuteChanged();
            });
        }

        private void OnGameOver(object? sender, EventArgs e)
        {
            OnModelUpdate(sender, e); 
            GameOver?.Invoke(this, e);
        }
        
        private void ExecuteNewGame()
        {
            _gameModel.StartGame();
            OnModelUpdate(this, EventArgs.Empty);
        }

        public void StartNewGame() { ExecuteNewGame(); }
        public void TogglePause() { ExecuteTogglePause(); }

        private void ExecuteTogglePause()
        {
            _gameModel.TogglePause();
            OnModelUpdate(this, EventArgs.Empty);
        }

        private async Task ExecuteSaveGameAsync()
        {
            try
            {
                string? filePath = _dialogService.ShowSaveDialog("Asteroid Game (*.save)|*.save", "save", _highScoreManager.GetSaveDirectory());
                if (!string.IsNullOrEmpty(filePath))
                {
                    var gameData = new GameData
                    {
                        Score = _gameModel.Score,
                        GameTime = _gameModel.GameTime,
                        SpaceshipX = _gameModel.Spaceship.X,
                        ScreenWidth = _gameModel.ScreenWidth,
                        ScreenHeight = _gameModel.ScreenHeight,
                        Asteroids = new List<AsteroidData>()
                    };

                    foreach (var asteroid in _gameModel.Asteroids)
                    {
                        gameData.Asteroids.Add(new AsteroidData { X = asteroid.X, Y = asteroid.Y, BaseSize = asteroid.BaseSize, Speed = asteroid.Speed });
                    }

                    await _persistence.SaveGameAsync(filePath, gameData).ConfigureAwait(false);

                    // Show dialog on UI thread
                    Dispatcher.UIThread.Post(() => _dialogService.ShowMessage("Game saved successfully!", "Save Game"));
                }
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Post(() => _dialogService.ShowMessage($"Error saving game: {ex.Message}", "Error"));
            }
        }

        private async Task ExecuteLoadGameAsync()
        {
            _gameModel.Stop(); 
            try
            {
                string? filePath = _dialogService.ShowOpenDialog("Asteroid Game (*.save)|*.save", "save", _highScoreManager.GetSaveDirectory());
                if (!string.IsNullOrEmpty(filePath))
                {
                    var gameData = await _persistence.LoadGameAsync(filePath).ConfigureAwait(false);

                    var asteroids = new List<Asteroid>();
                    foreach (var ad in gameData.Asteroids)
                    {
                        asteroids.Add(new Asteroid(ad.X, ad.Y, gameData.ScreenHeight, ad.BaseSize, ad.Speed));
                    }
                    
                    // Apply model changes on UI thread
                    Dispatcher.UIThread.Post(() =>
                    {
                        _gameModel = new GameModel(gameData.ScreenWidth, gameData.ScreenHeight, _highScoreManager, _persistence);
                        _gameModel.GameOver += OnGameOver;
                        _gameModel.ScoreChanged += OnModelUpdate;
                        _gameModel.GameTimeChanged += OnModelUpdate;
                        _gameModel.HighScoreChanged += OnModelUpdate;
                        
                        _gameModel.SetGameState(gameData.Score, gameData.GameTime, gameData.SpaceshipX, asteroids);

                        OnModelUpdate(this, EventArgs.Empty);
                        GameLoaded?.Invoke(this, EventArgs.Empty);
                        _dialogService.ShowMessage("Game loaded successfully! Press SPACE to resume.", "Load Game");
                    });
                }
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Post(() => _dialogService.ShowMessage($"Error loading game: {ex.Message}", "Error"));
            }
        }

        private void ExecuteResetHighScore()
        {
            if (_dialogService.ShowConfirmation("Are you sure you want to reset the high score?", "Reset High Score"))
            {
                try
                {
                    _highScoreManager.SaveHighScore(0);
                    _gameModel.SetHighScore(0); 
                    OnModelUpdate(this, EventArgs.Empty); 
                    _dialogService.ShowMessage("High score reset to 0!", "Reset High Score");
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage($"Error resetting high score: {ex.Message}", "Error");
                }
            }
        }

        private void ExecuteShowControls()
        {
            _dialogService.ShowMessage("Controls:\nArrows: Move\nSpace: Pause\nESC: Exit", "Game Controls");
        }

        private void ExecuteShowAbout()
        {
            _dialogService.ShowMessage("Asteroid Game Avalonia Port", "About");
        }
        
        public void SetMovingLeft(bool isMoving) => _gameModel.SetMovingLeft(isMoving);
        public void SetMovingRight(bool isMoving) => _gameModel.SetMovingRight(isMoving);
        public void StopGame() => Dispose();
        
        public void Dispose()
        {
            _gameModel.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}