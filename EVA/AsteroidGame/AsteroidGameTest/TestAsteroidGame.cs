using System;
using System.Collections.Generic;
using System.Linq;
using AsteroidGameMechanic.Model;
using System.Reflection; 
using Moq;
using AsteroidGameMechanic.Persistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsteroidGameTest
{
    [TestClass]
    public class GameModelTests : IDisposable
    {
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 600;
        private GameModel _gameModel = null!;
        private Mock<IHighScoreManager> _mockHighScoreManager = null!;
        private Mock<IGamePersistence> _mockGamePersistence = null!;

        private int _scoreChangedCount;
        private int _gameOverCount;
        private int _highScoreChangedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHighScoreManager = new Mock<IHighScoreManager>();
            _mockHighScoreManager.Setup(m => m.LoadHighScore()).Returns(0);
            _mockGamePersistence = new Mock<IGamePersistence>();
            _gameModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT, _mockHighScoreManager.Object,
                _mockGamePersistence.Object);

            _scoreChangedCount = 0;
            _gameOverCount = 0;
            _highScoreChangedCount = 0;

            _gameModel.ScoreChanged += (s, e) => _scoreChangedCount++;
            _gameModel.GameOver += (s, e) => _gameOverCount++;
            _gameModel.HighScoreChanged += (s, e) => _highScoreChangedCount++;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Keep existing cleanup for the test lifecycle
            _gameModel?.Dispose();
        }

        public void Dispose()
        {
            _gameModel?.Dispose();
            GC.SuppressFinalize(this);
        }

        private void CallGameTick(GameModel model, TimeSpan elapsed)
        {
            var method = typeof(GameModel).GetMethod("PerformGameTick",
                BindingFlags.Public | BindingFlags.Instance);

            Assert.IsNotNull(method, "A 'PerformGameTick' metódus nem található a GameModel-ben.");

            method?.Invoke(model, new object[] { elapsed });
        }

        [TestMethod]
        public void Test_GameInitialization()
        {
            Assert.AreEqual(0, _gameModel.Score);
            Assert.AreEqual(0, _gameModel.HighScore);
            Assert.IsFalse(_gameModel.IsGameOver);
            Assert.IsFalse(_gameModel.IsPaused);
            Assert.AreEqual(TimeSpan.Zero, _gameModel.GameTime);
            Assert.IsNotNull(_gameModel.Spaceship);
            Assert.IsNotNull(_gameModel.Asteroids);
            Assert.AreEqual(0, _gameModel.Asteroids.Count);
            Assert.AreEqual(SCREEN_WIDTH / 2, _gameModel.Spaceship.X);
            Assert.AreEqual(SCREEN_HEIGHT - 50, _gameModel.Spaceship.Y);

            _mockHighScoreManager.Verify(m => m.LoadHighScore(), Times.Once());
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Left()
        {
            var initialX = _gameModel.Spaceship.X;

            _gameModel.SetMovingLeft(true);
            CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));

            Console.Write(_gameModel.Spaceship.X);

            Assert.IsTrue(_gameModel.Spaceship.X < initialX);
            Assert.IsTrue(_gameModel.Spaceship.X >= 0);
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Right()
        {
            var initialX = _gameModel.Spaceship.X;

            _gameModel.SetMovingRight(true);
            CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));

            Assert.IsTrue(_gameModel.Spaceship.X > initialX);
            Assert.IsTrue(_gameModel.Spaceship.X <= SCREEN_WIDTH - _gameModel.Spaceship.Width);
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Boundaries()
        {
            _gameModel.SetMovingLeft(true);
            for (int i = 0; i < 100; i++)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));
            }

            Assert.AreEqual(0, _gameModel.Spaceship.X);
            _gameModel.SetMovingLeft(false);

            _gameModel.SetMovingRight(true);
            for (int i = 0; i < 10000; i++)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));
                _gameModel.Asteroids.Clear();
            }

            int expectedMaxX = SCREEN_WIDTH - _gameModel.Spaceship.Width;
            Assert.AreEqual(expectedMaxX, _gameModel.Spaceship.X);
        }

        [TestMethod]
        public void Test_AsteroidSpawning_OverTime()
        {
            var initialCount = _gameModel.Asteroids.Count;

            for (int i = 0; i < 100; i++)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));
            }

            Assert.IsTrue(_gameModel.Asteroids.Count > initialCount);
        }

        [TestMethod]
        public void Test_AsteroidSpawning_Positions()
        {
            var initialAsteroidCount = _gameModel.Asteroids.Count;

            int maxAttempts = 10;
            int attempt = 0;
            while (_gameModel.Asteroids.Count == initialAsteroidCount && attempt < maxAttempts)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(200));
                attempt++;
            }

            Assert.IsTrue(_gameModel.Asteroids.Count > initialAsteroidCount);

            var newAsteroids = _gameModel.Asteroids.Skip(initialAsteroidCount).ToList();
            foreach (var asteroid in newAsteroids)
            {
                Assert.IsTrue(asteroid.Y <= 0);
                Assert.IsTrue(asteroid.X >= 20);
                Assert.IsTrue(asteroid.X <= SCREEN_WIDTH - 50);
            }
        }


        [TestMethod]
        public void Test_CollisionDetection_GameOver()
        {
            var spaceship = _gameModel.Spaceship;

            var collidingAsteroid = new Asteroid(
                spaceship.X,
                spaceship.Y,
                SCREEN_HEIGHT,
                30,
                5
            );

            _gameModel.Asteroids.Add(collidingAsteroid);

            CallGameTick(_gameModel, TimeSpan.FromSeconds(1));

            Assert.IsTrue(_gameModel.IsGameOver);
            Assert.AreEqual(1, _gameOverCount);
        }

        [TestMethod]
        public void Test_CollisionDetection_NoCollision()
        {
            var safeAsteroid = new Asteroid(
                100,
                100,
                SCREEN_HEIGHT,
                30,
                5
            );

            _gameModel.Asteroids.Add(safeAsteroid);

            CallGameTick(_gameModel, TimeSpan.FromSeconds(1));

            Assert.IsFalse(_gameModel.IsGameOver);
            Assert.AreEqual(0, _gameOverCount);
        }

        [TestMethod]
        public void Test_ScoringSystem_AsteroidPassing()
        {
            var initialScore = _gameModel.Score;
            var initialEventCount = _scoreChangedCount;

            var passingAsteroid = new Asteroid(
                SCREEN_WIDTH / 2,
                SCREEN_HEIGHT - 1,
                SCREEN_HEIGHT,
                30,
                5
            );

            _gameModel.Asteroids.Add(passingAsteroid);

            CallGameTick(_gameModel, TimeSpan.FromSeconds(0.5));

            Assert.IsTrue(_gameModel.Score > initialScore);
            Assert.IsTrue(_scoreChangedCount > initialEventCount);
        }

        [TestMethod]
        public void Test_PauseFunctionality_Toggle()
        {
            Assert.IsFalse(_gameModel.IsPaused);

            _gameModel.TogglePause();

            Assert.IsTrue(_gameModel.IsPaused);

            _gameModel.TogglePause();

            Assert.IsFalse(_gameModel.IsPaused);
        }

        [TestMethod]
        public void Test_PauseFunctionality_NoMovementWhenPaused()
        {
            var initialX = _gameModel.Spaceship.X;
            _gameModel.TogglePause();

            _gameModel.SetMovingLeft(true);
            _gameModel.SetMovingRight(true);
            CallGameTick(_gameModel, TimeSpan.FromSeconds(0.1));

            Assert.AreEqual(initialX, _gameModel.Spaceship.X);
        }

        [TestMethod]
        public void Test_PauseFunctionality_NoUpdateWhenPaused()
        {
            var asteroid = new Asteroid(100, 100, SCREEN_HEIGHT, 30, 5);
            _gameModel.Asteroids.Add(asteroid);
            var initialY = asteroid.Y;

            _gameModel.TogglePause();

            CallGameTick(_gameModel, TimeSpan.FromSeconds(1));

            Assert.AreEqual(initialY, asteroid.Y);
        }

        [TestMethod]
        public void Test_HighScoreTracking_NewHighScore()
        {
            var mockMgr = new Mock<IHighScoreManager>();
            mockMgr.Setup(m => m.LoadHighScore()).Returns(100);
            var mockPersistence = new Mock<IGamePersistence>();
            var highScoreModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT, mockMgr.Object, mockPersistence.Object);

            int highScoreChanged = 0;
            highScoreModel.HighScoreChanged += (s, e) => highScoreChanged++;

            SetPrivateField(highScoreModel, "_score", 150);

            var collidingAsteroid = new Asteroid(
                highScoreModel.Spaceship.X,
                highScoreModel.Spaceship.Y,
                SCREEN_HEIGHT,
                30,
                5
            );
            highScoreModel.Asteroids.Add(collidingAsteroid);

            CallGameTick(highScoreModel, TimeSpan.FromSeconds(1));

            Assert.IsTrue(highScoreModel.IsGameOver);
            Assert.AreEqual(150, highScoreModel.HighScore);
            Assert.AreEqual(1, highScoreChanged);

            mockMgr.Verify(m => m.SaveHighScore(150), Times.Once());
        }

        [TestMethod]
        public void Test_HighScoreTracking_NoUpdateWhenLower()
        {
            var mockMgr = new Mock<IHighScoreManager>();
            mockMgr.Setup(m => m.LoadHighScore()).Returns(100);
            var mockPersistence = new Mock<IGamePersistence>();
            var highScoreModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT, mockMgr.Object, mockPersistence.Object);

            int highScoreChanged = 0;
            highScoreModel.HighScoreChanged += (s, e) => highScoreChanged++;

            SetPrivateField(highScoreModel, "_score", 50);

            var collidingAsteroid = new Asteroid(
                highScoreModel.Spaceship.X,
                highScoreModel.Spaceship.Y,
                SCREEN_HEIGHT,
                30,
                5
            );
            highScoreModel.Asteroids.Add(collidingAsteroid);

            CallGameTick(highScoreModel, TimeSpan.FromSeconds(1));

            Assert.IsTrue(highScoreModel.IsGameOver);
            Assert.AreEqual(100, highScoreModel.HighScore);
            Assert.AreEqual(0, highScoreChanged);

            mockMgr.Verify(m => m.SaveHighScore(It.IsAny<int>()), Times.Never());
        }

        [TestMethod]
        public void Test_DifficultyProgression_OverTime()
        {
            var initialAsteroidCount = _gameModel.Asteroids.Count;

            for (int i = 0; i < 200; i++)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(0.5));
            }

            Assert.IsTrue(_gameModel.Asteroids.Count > initialAsteroidCount);

            var difficulty = _gameModel.GetDifficultyDescription();
            Assert.IsFalse(string.IsNullOrEmpty(difficulty));
        }

        [TestMethod]
        public void Test_GameStatePersistence_SetState()
        {
            var asteroids = new List<Asteroid>
            {
                new Asteroid(100, 200, SCREEN_HEIGHT, 25, 5),
                new Asteroid(300, 400, SCREEN_HEIGHT, 40, 3)
            };

            _gameModel.SetGameState(
                score: 500,
                gameTime: TimeSpan.FromMinutes(2),
                spaceshipX: 350,
                asteroids: asteroids
            );

            Assert.AreEqual(500, _gameModel.Score);
            Assert.AreEqual(TimeSpan.FromMinutes(2), _gameModel.GameTime);
            Assert.AreEqual(350, _gameModel.Spaceship.X);
            Assert.AreEqual(2, _gameModel.Asteroids.Count);
            Assert.IsTrue(_gameModel.IsPaused);
            Assert.IsFalse(_gameModel.IsGameOver);
        }

        [TestMethod]
        public void Test_AsteroidSizeVariation_Types()
        {
            var asteroidSizes = new List<int>();

            for (int i = 0; i < 1000; i++)
            {
                CallGameTick(_gameModel, TimeSpan.FromSeconds(0.5));

                foreach (var asteroid in _gameModel.Asteroids)
                {
                    asteroidSizes.Add(asteroid.Width);
                }
            }

            if (asteroidSizes.Count > 0)
            {
                var minSize = asteroidSizes.Min();
                var maxSize = asteroidSizes.Max();

                Assert.IsTrue(minSize < maxSize);

                Assert.IsTrue(minSize >= 10);
                Assert.IsTrue(maxSize <= 100);

                var smallAsteroids = asteroidSizes.Count(size => size < 25);
                var mediumAsteroids = asteroidSizes.Count(size => size >= 25 && size < 40);
                var largeAsteroids = asteroidSizes.Count(size => size >= 40 && size < 60);
                var giantAsteroids = asteroidSizes.Count(size => size >= 60);

                var sizeCategories = new[] { smallAsteroids, mediumAsteroids, largeAsteroids, giantAsteroids }
                    .Count(count => count > 0);

                Assert.IsTrue(sizeCategories >= 2);
            }
            else
            {
                Assert.Inconclusive("No asteroids spawned during test - may need more iterations");
            }
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(obj, value);
        }

        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (T)field!.GetValue(obj)!;
        }
    }
}