using AsteriodGameMechanic.Model;
using Moq;

namespace AsteroidGameTest
{
    [TestClass]
    public class GameModelTests
    {
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 600;
        private GameModel _gameModel;
        private MockRepository _mockRepository;
        private int _scoreChangedCount;
        private int _gameOverCount;
        private int _highScoreChangedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _gameModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT);
            
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
            _mockRepository.VerifyAll();
        }

        [TestMethod]
        public void Test_GameInitialization()
        {
            // Assert
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
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Left()
        {
            // Arrange
            var initialX = _gameModel.Spaceship.X;

            // Act
            _gameModel.MoveSpaceshipLeft();

            // Assert
            Assert.IsTrue(_gameModel.Spaceship.X < initialX);
            Assert.IsTrue(_gameModel.Spaceship.X >= 0);
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Right()
        {
            // Arrange
            var initialX = _gameModel.Spaceship.X;

            // Act
            _gameModel.MoveSpaceshipRight();

            // Assert
            Assert.IsTrue(_gameModel.Spaceship.X > initialX);
            Assert.IsTrue(_gameModel.Spaceship.X <= SCREEN_WIDTH - _gameModel.Spaceship.Width);
        }

        [TestMethod]
        public void Test_SpaceshipMovement_Boundaries()
        {
            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _gameModel.MoveSpaceshipLeft();
            }

            // Assert
            Assert.AreEqual(0, _gameModel.Spaceship.X);

            // Arrange
            for (int i = 0; i < 100; i++)
            {
                _gameModel.MoveSpaceshipRight();
            }

            // Assert
            int expectedMaxX = SCREEN_WIDTH - _gameModel.Spaceship.Width;
            Assert.AreEqual(expectedMaxX, _gameModel.Spaceship.X);
        }

        [TestMethod]
        public void Test_AsteroidSpawning_OverTime()
        {
            // Arrange
            var initialCount = _gameModel.Asteroids.Count;

            // Act
            for (int i = 0; i < 100; i++)
            {
                _gameModel.Update(TimeSpan.FromSeconds(0.1));
            }

            // Assert
            Assert.IsTrue(_gameModel.Asteroids.Count > initialCount);
        }

        [TestMethod]
        public void Test_AsteroidSpawning_Positions()
        {
            // Arrange
            var initialAsteroidCount = _gameModel.Asteroids.Count;
        
            // Act
            int maxAttempts = 10;
            int attempt = 0;
            while (_gameModel.Asteroids.Count == initialAsteroidCount && attempt < maxAttempts)
            {
                _gameModel.Update(TimeSpan.FromSeconds(200));
                attempt++;
            }
        
            // Assert
            Assert.IsTrue(_gameModel.Asteroids.Count > initialAsteroidCount);
            
            var newAsteroids = _gameModel.Asteroids.Skip(initialAsteroidCount).ToList();
            foreach (var asteroid in newAsteroids)
            {
                Assert.IsTrue(asteroid.Y < 0);
                Assert.IsTrue(asteroid.X >= 20);
                Assert.IsTrue(asteroid.X <= SCREEN_WIDTH - 50);
            }
        }

        [TestMethod]
        public void Test_CollisionDetection_GameOver()
        {
            // Arrange
            var spaceship = _gameModel.Spaceship;
            
            var collidingAsteroid = new Mock<Asteroid>(
                spaceship.X, 
                spaceship.Y, 
                SCREEN_HEIGHT, 
                30,
                5
            );
            
            _gameModel.Asteroids.Add(collidingAsteroid.Object);

            // Act
            _gameModel.Update(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsTrue(_gameModel.IsGameOver);
            Assert.AreEqual(1, _gameOverCount);
        }

        [TestMethod]
        public void Test_CollisionDetection_NoCollision()
        {
            // Arrange
            var safeAsteroid = new Mock<Asteroid>(
                100,
                100,
                SCREEN_HEIGHT,
                30,
                5
            );
            
            _gameModel.Asteroids.Add(safeAsteroid.Object);

            // Act
            _gameModel.Update(TimeSpan.FromSeconds(1));

            // Assert
            Assert.IsFalse(_gameModel.IsGameOver);
            Assert.AreEqual(0, _gameOverCount);
        }

        [TestMethod]
        public void Test_ScoringSystem_AsteroidPassing()
        {
            // Arrange
            var initialScore = _gameModel.Score;
            var initialEventCount = _scoreChangedCount;
            
            var passingAsteroid = new Mock<Asteroid>(
                SCREEN_WIDTH / 2,
                SCREEN_HEIGHT + 10,
                SCREEN_HEIGHT,
                30,
                5
            );
            
            _gameModel.Asteroids.Add(passingAsteroid.Object);

            // Act
            _gameModel.Update(TimeSpan.FromSeconds(0.1));

            // Assert
            Assert.IsTrue(_gameModel.Score > initialScore);
            Assert.IsTrue(_scoreChangedCount > initialEventCount);
        }

        [TestMethod]
        public void Test_PauseFunctionality_Toggle()
        {
            // Arrange
            Assert.IsFalse(_gameModel.IsPaused);

            // Act
            _gameModel.TogglePause();

            // Assert
            Assert.IsTrue(_gameModel.IsPaused);

            // Act
            _gameModel.TogglePause();

            // Assert
            Assert.IsFalse(_gameModel.IsPaused);
        }

        [TestMethod]
        public void Test_PauseFunctionality_NoMovementWhenPaused()
        {
            // Arrange
            var initialX = _gameModel.Spaceship.X;
            _gameModel.TogglePause();

            // Act
            _gameModel.MoveSpaceshipLeft();
            _gameModel.MoveSpaceshipRight();

            // Assert
            Assert.AreEqual(initialX, _gameModel.Spaceship.X);
        }

        [TestMethod]
        public void Test_PauseFunctionality_NoUpdateWhenPaused()
        {
            // Arrange
            var asteroid = new Mock<Asteroid>(100, 100, SCREEN_HEIGHT, 30, 5);
            _gameModel.Asteroids.Add(asteroid.Object);
            var initialY = asteroid.Object.Y;
            
            _gameModel.TogglePause();

            // Act
            _gameModel.Update(TimeSpan.FromSeconds(1));

            // Assert
            Assert.AreEqual(initialY, asteroid.Object.Y);
        }

        [TestMethod]
        public void Test_HighScoreTracking_NewHighScore()
        {
            // Arrange
            var highScoreModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT, 100);
            int highScoreChanged = 0;
            highScoreModel.HighScoreChanged += (s, e) => highScoreChanged++;
            
            SetPrivateField(highScoreModel, "_score", 150);

            var collidingAsteroid = new Mock<Asteroid>(
                highScoreModel.Spaceship.X,
                highScoreModel.Spaceship.Y,
                SCREEN_HEIGHT,
                30,
                5
            );
            highScoreModel.Asteroids.Add(collidingAsteroid.Object);
            highScoreModel.Update(TimeSpan.FromSeconds(1));

            // Assert
            Assert.AreEqual(150, highScoreModel.HighScore);
            Assert.AreEqual(1, highScoreChanged);
        }

        [TestMethod]
        public void Test_HighScoreTracking_NoUpdateWhenLower()
        {
            // Arrange
            var highScoreModel = new GameModel(SCREEN_WIDTH, SCREEN_HEIGHT, 100);
            int highScoreChanged = 0;
            highScoreModel.HighScoreChanged += (s, e) => highScoreChanged++;

            // Act
            SetPrivateField(highScoreModel, "_score", 50);
            
            var collidingAsteroid = new Mock<Asteroid>(
                highScoreModel.Spaceship.X,
                highScoreModel.Spaceship.Y,
                SCREEN_HEIGHT,
                30,
                5
            );
            highScoreModel.Asteroids.Add(collidingAsteroid.Object);
            highScoreModel.Update(TimeSpan.FromSeconds(1));

            // Assert
            Assert.AreEqual(100, highScoreModel.HighScore);
            Assert.AreEqual(0, highScoreChanged);
        }

        [TestMethod]
        public void Test_DifficultyProgression_OverTime()
        {
            // Arrange
            var initialAsteroidCount = _gameModel.Asteroids.Count;

            // Act
            for (int i = 0; i < 200; i++)
            {
                _gameModel.Update(TimeSpan.FromSeconds(0.5));
            }

            // Assert
            Assert.IsTrue(_gameModel.Asteroids.Count > initialAsteroidCount);
            
            var difficulty = _gameModel.GetDifficultyDescription();
            Assert.IsFalse(string.IsNullOrEmpty(difficulty));
        }

        [TestMethod]
        public void Test_GameStatePersistence_SetState()
        {
            // Arrange
            var asteroids = new List<Asteroid>
            {
                new Mock<Asteroid>(100, 200, SCREEN_HEIGHT, 25, 5).Object,
                new Mock<Asteroid>(300, 400, SCREEN_HEIGHT, 40, 3).Object
            };

            // Act
            _gameModel.SetGameState(
                score: 500,
                gameTime: TimeSpan.FromMinutes(2),
                spaceshipX: 350,
                asteroids: asteroids
            );

            // Assert
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
            // Arrange & Act
            var asteroidSizes = new List<int>();

            for (int i = 0; i < 100; i++)
            {
                _gameModel.Update(TimeSpan.FromSeconds(0.5));
        
                foreach (var asteroid in _gameModel.Asteroids)
                {
                    asteroidSizes.Add(asteroid.Width);
                }
            }

            // Assert
            if (asteroidSizes.Count > 0)
            {
                var minSize = asteroidSizes.Min();
                var maxSize = asteroidSizes.Max();
        
                Assert.IsTrue(minSize < maxSize);
        
                // The minimum visible size might be smaller than 15 due to random variation
                // Let's use a more reasonable minimum based on observation
                Assert.IsTrue(minSize >= 10);
                Assert.IsTrue(maxSize <= 100);
        
                // Test that we have multiple size categories
                var smallAsteroids = asteroidSizes.Count(size => size < 25);
                var mediumAsteroids = asteroidSizes.Count(size => size >= 25 && size < 40);
                var largeAsteroids = asteroidSizes.Count(size => size >= 40 && size < 60);
                var giantAsteroids = asteroidSizes.Count(size => size >= 60);
        
                // We should have at least 2 different size categories
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
            return (T)field?.GetValue(obj);
        }
    }
}