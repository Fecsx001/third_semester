using System;
using System.Collections.Generic;
using System.Drawing;

namespace AsteroidGame.Model
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

        public event EventHandler GameOver;
        public event EventHandler ScoreChanged;
        public event EventHandler GameTimeChanged;

        public GameModel(int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            _random = new Random();
            Asteroids = new List<Asteroid>();
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

            // Generate new asteroids based on game time
            double spawnProbability = Math.Min(0.05 + (_gameTime.TotalSeconds * 0.0005), 0.2);
            if (_random.NextDouble() < spawnProbability)
            {
                int x = _random.Next(20, ScreenWidth - 50);
                Asteroids.Add(new Asteroid(x, -30, ScreenHeight));
            }

            // Move asteroids and check collisions
            for (int i = Asteroids.Count - 1; i >= 0; i--)
            {
                Asteroids[i].Move();

                // Check collision with spaceship
                if (CheckCollision(Spaceship, Asteroids[i]))
                {
                    _isGameOver = true;
                    GameOver?.Invoke(this, EventArgs.Empty);
                    return;
                }

                // Remove asteroids that are out of screen
                if (Asteroids[i].Y > ScreenHeight)
                {
                    Asteroids.RemoveAt(i);
                    _score += 10;
                    ScoreChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool CheckCollision(Spaceship spaceship, Asteroid asteroid)
        {
            Rectangle shipRect = new Rectangle(spaceship.X, spaceship.Y, spaceship.Width, spaceship.Height);
            Rectangle asteroidRect = new Rectangle(asteroid.X, asteroid.Y, asteroid.Width, asteroid.Height);
            return shipRect.IntersectsWith(asteroidRect);
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
    }
}