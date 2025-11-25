using Microsoft.VisualStudio.TestTools.UnitTesting;
using AsteroidGameAvalonia.Model;
using AsteroidGameAvalonia.Persistance;
using Moq;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsteroidGameAvalonia.Tests
{
    [TestClass]
    public class TestPersistence
    {
        private string _testDirectory = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _testDirectory = Path.Combine(Path.GetTempPath(), "AsteroidTest_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDirectory);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(_testDirectory))
            {
                Directory.Delete(_testDirectory, true);
            }
        }

        [TestMethod]
        public void HighScoreManager_LoadHighScore_ReturnsZeroForNonExistentFile()
        {
            var manager = new Moq.Mock<IHighScoreManager>();
            manager.Setup(m => m.LoadHighScore()).Returns(0);

            int score = manager.Object.LoadHighScore();

            Assert.AreEqual(0, score, "A HighScore-nak 0-nak kell lennie, ha a fájl nem létezik.");
        }

        [TestMethod]
        public void HighScoreManager_LoadHighScore()
        {
            var manager = new Mock<IHighScoreManager>();
            manager.Setup(m => m.LoadHighScore()).Returns(500);

            int score = manager.Object.LoadHighScore();

            Assert.AreEqual(500, score, "A betöltött pontszám nem egyezik a mentettel.");
        }
        

        [TestMethod]
        public async Task GamePersistence_Load()
        {
            var manager = new Mock<IGamePersistence>();
            manager.Setup(m => m.LoadGameAsync(_testDirectory)).ReturnsAsync(new GameData
            {
                Score = 1000,
                GameTime = TimeSpan.FromSeconds(60),
                SpaceshipX = 400,
                ScreenWidth = 800,
                ScreenHeight = 600,
                Asteroids = new List<AsteroidData>
                {
                    new AsteroidData { X = 100, Y = 200, BaseSize = 30, Speed = 5 },
                    new AsteroidData { X = 150, Y = 250, BaseSize = 50, Speed = 2 }
                }
            });
            manager.Setup(m => m.SaveGameAsync(It.IsAny<string>(), It.IsAny<GameData>())).Verifiable();
            
            var highscore = new Mock<IHighScoreManager>();
            highscore.Setup(m => m.LoadHighScore()).Returns(500);

            var originalModel = new GameModel(800, 600, highscore.Object, manager.Object);

            var asteroids = new List<Asteroid>
            {
                new Asteroid(100, 200, 600, 30, 5),
                new Asteroid(150, 250, 600, 50, 2)
            };
            originalModel.SetGameState(1000, TimeSpan.FromSeconds(60), 400, asteroids);

            GameData loadedData = await manager.Object.LoadGameAsync(_testDirectory);

            Assert.IsNotNull(loadedData);
            Assert.AreEqual(originalModel.Score, loadedData.Score);
            Assert.AreEqual(originalModel.GameTime, loadedData.GameTime);
            Assert.AreEqual(originalModel.Spaceship.X, loadedData.SpaceshipX);
            Assert.AreEqual(originalModel.ScreenWidth, loadedData.ScreenWidth);
            Assert.AreEqual(originalModel.ScreenHeight, loadedData.ScreenHeight);

            Assert.AreEqual(2, loadedData.Asteroids.Count, "Az aszteroidák száma nem egyezik.");
            
            Assert.AreEqual(asteroids[0].X, loadedData.Asteroids[0].X);
            Assert.AreEqual(asteroids[0].Y, loadedData.Asteroids[0].Y);
            Assert.AreEqual(30, loadedData.Asteroids[0].BaseSize);
            Assert.AreEqual(5, loadedData.Asteroids[0].Speed);

            Assert.AreEqual(asteroids[1].X, loadedData.Asteroids[1].X);
            Assert.AreEqual(50, loadedData.Asteroids[1].BaseSize);
        }
    }
}