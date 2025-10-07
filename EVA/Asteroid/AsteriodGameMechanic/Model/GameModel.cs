using System;
using System.Collections.Generic;
using System.Drawing;

namespace AsteriodGameMechanic.Model
{
    public class GameModel
    {
        private Random _random;
        private int _score;
        private bool _isGameOver;
        private bool _isPaused;
        private TimeSpan _gameTime;
        
        public Spaceship Spaceship { get; private set; }
        public List<Asteroid> Asteroids { get; private set; }
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public int Score => _score;
        public bool IsGameOver => _isGameOver;
        public bool IsPaused => _isPaused;
        public TimeSpan GameTime => _gameTime;
        public int HighScore { get; private set; }

        public event EventHandler GameOver;
        public event EventHandler ScoreChanged;
        public event EventHandler GameTimeChanged;
        public event EventHandler HighScoreChanged;

        public GameModel(int screenWidth, int screenHeight, int initialHighScore = 0)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            _random = new Random();
            Asteroids = new List<Asteroid>();
            HighScore = initialHighScore;
            InitializeGame();
        }

        public void InitializeGame()
        {
            Spaceship = new Spaceship(ScreenWidth / 2, ScreenHeight - 50, ScreenWidth);
            Asteroids.Clear();
            _score = 0;
            _isGameOver = false;
            _isPaused = false;
            _gameTime = TimeSpan.Zero;
            
            ScoreChanged?.Invoke(this, EventArgs.Empty);
            GameTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update(TimeSpan elapsedTime)
        {
            if (_isPaused || _isGameOver) return;

            // Update game time
            _gameTime += elapsedTime;
            GameTimeChanged?.Invoke(this, EventArgs.Empty);

            // Calculate difficulty factors based on time and score
            double timeFactor = Math.Min(_gameTime.TotalSeconds / 60.0, 2.0);
            double scoreFactor = Math.Min(_score / 1000.0, 2.0);
            double difficulty = 1.0 + (timeFactor + scoreFactor) / 2.0;

            // Generate new asteroids with varying sizes and speeds based on difficulty
            double spawnProbability = Math.Min(0.05 + (_gameTime.TotalSeconds * 0.0003), 0.3);
            spawnProbability *= difficulty;

            if (_random.NextDouble() < spawnProbability)
            {
                int x = _random.Next(20, ScreenWidth - 50);
                
                // Determine asteroid type based on probability
                double typeRandom = _random.NextDouble();
                int baseSize;
                int speed;

                if (typeRandom < 0.4) // 40% chance: Small fast asteroids
                {
                    baseSize = _random.Next(15, 25);
                    speed = (int)(5 + difficulty * 3);
                }
                else if (typeRandom < 0.7) // 30% chance: Medium balanced asteroids
                {
                    baseSize = _random.Next(25, 40);
                    speed = (int)(4 + difficulty * 2);
                }
                else if (typeRandom < 0.9) // 20% chance: Large slow asteroids
                {
                    baseSize = _random.Next(40, 60);
                    speed = (int)(3 + difficulty * 1);
                }
                else // 10% chance: Giant very slow asteroids
                {
                    baseSize = _random.Next(60, 80);
                    speed = (int)(2 + difficulty * 0.5);
                }

                speed = Math.Min(speed, 12);
                baseSize = Math.Max(15, baseSize);

                Asteroids.Add(new Asteroid(x, -baseSize, ScreenHeight, baseSize, speed));
            }

            // Move asteroids and check collisions
            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                Asteroids[i].Move();

                if (CheckCollision(Spaceship, Asteroids[i]))
                {
                    _isGameOver = true;
                    
                    // Check if this is a new high score
                    if (_score > HighScore)
                    {
                        HighScore = _score;
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

        // ... rest of the existing methods remain the same ...
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
            //return false; //Only for testing high scores
        }

        public void MoveSpaceshipLeft()
        {
            if (!_isPaused && !_isGameOver)
                Spaceship.MoveLeft();
        }

        public void MoveSpaceshipRight()
        {
            if (!_isPaused && !_isGameOver)
                Spaceship.MoveRight();
        }

        public void TogglePause()
        {
            if (!_isGameOver)
            {
                _isPaused = !_isPaused;
            }
        }

        public void SetGameState(int score, TimeSpan gameTime, int spaceshipX, List<Asteroid> asteroids)
        {
            _score = score;
            _gameTime = gameTime;
            _isGameOver = false;
            _isPaused = true;
            
            Spaceship = new Spaceship(spaceshipX, ScreenHeight - 50, ScreenWidth);
            Asteroids.Clear();
            Asteroids.AddRange(asteroids);
            
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

        public string GetAsteroidDistribution()
        {
            int small = 0, medium = 0, large = 0, giant = 0;
            
            foreach (var asteroid in Asteroids)
            {
                int size = GetAsteroidBaseSize(asteroid);
                if (size < 25) small++;
                else if (size < 40) medium++;
                else if (size < 60) large++;
                else giant++;
            }
            
            return $"S:{small} M:{medium} L:{large} G:{giant}";
        }
    }
}