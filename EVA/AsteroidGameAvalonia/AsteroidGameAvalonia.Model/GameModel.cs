using System.Drawing;
using System.Security.Principal;
using System.Timers;
using AsteroidGameAvalonia.Persistance;

namespace AsteroidGameAvalonia.Model
{
    public class GameModel : IDisposable
    {
        private readonly IHighScoreManager _highScoreManager;
        private readonly IGamePersistence _gamePersistence;
        private readonly Random _random;
        private int _score;
        private bool _isGameOver;
        private bool _isPaused;
        private TimeSpan _gameTime;

        private System.Timers.Timer _timer;
        private const int TickInterval = 16;
        private DateTime _lastUpdateTime;
        private bool _isMovingLeft;
        private bool _isMovingRight;
        public Spaceship Spaceship { get; private set; } = null!;
        public List<Asteroid> Asteroids { get; private set; }
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public int Score => _score;
        public bool IsGameOver => _isGameOver;
        public bool IsPaused => _isPaused;
        public TimeSpan GameTime => _gameTime;
        public int HighScore { get; private set; }

        public event EventHandler? GameOver;
        public event EventHandler? ScoreChanged;
        public event EventHandler? GameTimeChanged;
        public event EventHandler? HighScoreChanged;

        public GameModel(int screenWidth, int screenHeight, IHighScoreManager highScoreManager, IGamePersistence gamePersistence)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            _highScoreManager = highScoreManager;
            _gamePersistence = gamePersistence;
            _random = new Random();
            Asteroids = new List<Asteroid>();
            HighScore = _highScoreManager.LoadHighScore();
            
            _timer = new System.Timers.Timer();
            _timer.Elapsed += OnTimerElapsed;
            _timer.Interval = TickInterval;
            _timer.AutoReset = true;
            _timer.Stop(); 
            InitializeGame();
        }

        private void InitializeGame()
        {
            Spaceship = new Spaceship(ScreenWidth / 2, ScreenHeight - 50, ScreenWidth);
            Asteroids.Clear();
            _score = 0;
            _isGameOver = false;
            _isPaused = false;
            _gameTime = TimeSpan.Zero;
            
            _isMovingLeft = false;
            _isMovingRight = false;
            
            ScoreChanged?.Invoke(this, EventArgs.Empty);
            GameTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void PerformGameTick(TimeSpan elapsedTime)
        {
            if (_isPaused || _isGameOver) return;
            if (_isMovingLeft) Spaceship.MoveLeft();
            if (_isMovingRight) Spaceship.MoveRight();
            
            _gameTime += elapsedTime;
            GameTimeChanged?.Invoke(this, EventArgs.Empty);
            
            double timeFactor = Math.Min(_gameTime.TotalSeconds / 60.0, 2.0);
            double scoreFactor = Math.Min(_score / 1000.0, 2.0);
            double difficulty = 1.0 + (timeFactor + scoreFactor) / 2.0;
            
            double spawnProbability = Math.Min(0.05 + (_gameTime.TotalSeconds * 0.0003), 0.3);
            spawnProbability *= difficulty;

            if (_random.NextDouble() < spawnProbability)
            {
                int x = _random.Next(20, ScreenWidth - 50);
                
                double typeRandom = _random.NextDouble();
                int baseSize;
                int speed;

                if (typeRandom < 0.4)
                {
                    baseSize = _random.Next(15, 25);
                    speed = (int)(5 + difficulty * 3);
                }
                else if (typeRandom < 0.7)
                {
                    baseSize = _random.Next(25, 40);
                    speed = (int)(4 + difficulty * 2);
                }
                else if (typeRandom < 0.9)
                {
                    baseSize = _random.Next(40, 60);
                    speed = (int)(3 + difficulty * 1);
                }
                else
                {
                    baseSize = _random.Next(60, 80);
                    speed = (int)(2 + difficulty * 0.5);
                }

                speed = Math.Min(speed, 12);
                baseSize = Math.Max(15, baseSize);

                int spawnY = -baseSize;
                Asteroids.Add(new Asteroid(x, spawnY, ScreenHeight, baseSize, speed));
            }
            
            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                Asteroids[i].Move();

                if (CheckCollision(Spaceship, Asteroids[i]))
                {
                    _isGameOver = true;
                    _timer.Stop();

                    if (_score > HighScore)
                    {
                        HighScore = _score;
                        _highScoreManager.SaveHighScore(_score);
                        HighScoreChanged?.Invoke(this, EventArgs.Empty);
                    }
                    
                    GameOver?.Invoke(this, EventArgs.Empty);
                    return;
                }

                if (Asteroids[i].Y > ScreenHeight)
                {
                    int baseSize = GetAsteroidBaseSize(Asteroids[i]);
                    int speed = GetAsteroidSpeed(Asteroids[i]);
                    
                    int points = 30 + (40 - baseSize) + (speed - 3) * 2;
                    points = Math.Max(10, points);
                    
                    Asteroids.RemoveAt(i);
                    _score += points;
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void SetHighScore(int highScore)
        {
            HighScore = highScore;
            HighScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        private int GetAsteroidBaseSize(Asteroid asteroid)
        {
            return (asteroid.Width + asteroid.Height) / 2;
        }

        private int GetAsteroidSpeed(Asteroid asteroid)
        {
            int baseSize = GetAsteroidBaseSize(asteroid);
            if (baseSize >= 60) return 2;
            if (baseSize >= 40) return 3;
            if (baseSize >= 25) return 4;
            return 5;
        }

        private bool CheckCollision(Spaceship spaceship, Asteroid asteroid)
        {
            Rectangle shipRect = new Rectangle(spaceship.X, spaceship.Y, spaceship.Width, spaceship.Height);
            Rectangle asteroidRect = new Rectangle(asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
            return shipRect.IntersectsWith(asteroidRect);
        }

        public void SetMovingLeft(bool isMoving)
        {
            _isMovingLeft = isMoving;
        }
        public void SetMovingRight(bool isMoving)
        {
            _isMovingRight = isMoving;
        }
        public void Stop()
        {
            _timer.Stop(); 
            _timer.Dispose(); 
        }

        public void TogglePause()
        {
            if (!_isGameOver)
            {
                _isPaused = !_isPaused;
                
                if (_isPaused)
                {
                    _timer.Stop();
                }
                else
                {
                    _lastUpdateTime = DateTime.Now; 
                    _timer.Start();
                }
            }
        }

        public void SetGameState(int score, TimeSpan gameTime, int spaceshipX, List<Asteroid> asteroids)
        {
            // _timer.Stop(); // <-- EZT A SORT TÖRÖLD KI
            _score = score;
            _gameTime = gameTime;
            _isGameOver = false;
            _isPaused = true;
            
            Spaceship = new Spaceship(spaceshipX, ScreenHeight - 50, ScreenWidth);
            Asteroids = asteroids;
            
            ScoreChanged?.Invoke(this, EventArgs.Empty);
            GameTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetDifficultyDescription()
        {
            double timeFactor = Math.Min(_gameTime.TotalSeconds / 60.0, 2.0);
            double scoreFactor = Math.Min(_score / 1000.0, 2.0);
            double difficulty = 1.0 + (timeFactor + scoreFactor) / 2.0;

            if (difficulty < 1.5) return "Easy";
            if (difficulty < 2.0) return "Medium";
            if (difficulty < 2.5) return "Hard";
            return "Extreme";
        }
        public void StartGame()
        {
            InitializeGame();
            _lastUpdateTime = DateTime.Now;
            _timer.Start();
        }
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_isPaused || _isGameOver) return;

            DateTime currentTime = DateTime.Now;
            TimeSpan elapsedTime = currentTime - _lastUpdateTime;
            _lastUpdateTime = currentTime;
 
            PerformGameTick(elapsedTime);
        }
        
        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
    }
}