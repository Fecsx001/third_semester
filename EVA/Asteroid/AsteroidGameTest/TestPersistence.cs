using Microsoft.VisualStudio.TestTools.UnitTesting;
using AsteriodGameMechanic.Persistence;
using AsteriodGameMechanic.Model;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsteroidGameTest
{
    [TestClass]
    public class TestPersistence
    {
        private string _testDirectory;

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
            var manager = new HighScoreManager(_testDirectory);

            // Act
            int score = manager.LoadHighScore();

            // Assert
            Assert.AreEqual(0, score, "A HighScore-nak 0-nak kell lennie, ha a fájl nem létezik.");
        }

        [TestMethod]
        public void HighScoreManager_SaveAndLoadHighScore_SuccessfulRoundtrip()
        {
            // Arrange
            var manager = new HighScoreManager(_testDirectory);
            int expectedScore = 500;

            // Act
            manager.SaveHighScore(expectedScore);

            int loadedScore = manager.LoadHighScore();

            // Assert
            Assert.AreEqual(expectedScore, loadedScore, "A betöltött pontszám nem egyezik a mentettel.");
            
            // 3.
            string expectedFile = Path.Combine(_testDirectory, "SaveData", "highscore.txt");
            Assert.IsTrue(File.Exists(expectedFile), "A highscore.txt fájl nem jött létre.");
        }

        [TestMethod]
        public void HighScoreManager_SaveHighScore_OnlySavesIfHigher()
        {
            // Arrange
            var manager = new HighScoreManager(_testDirectory);
            int initialScore = 1000;
            int lowerScore = 500;

            // Act
            manager.SaveHighScore(initialScore);

            manager.SaveHighScore(lowerScore);

            // Assert
            int loadedScore = manager.LoadHighScore();
            Assert.AreEqual(initialScore, loadedScore, "A pontszámot felülírta, pedig az új alacsonyabb volt.");
        }

        [TestMethod]
        public void GamePersistence_SaveAndLoad_SuccessfulRoundtrip()
        {
            // Arrange
            var persistence = new GamePersistence();
            string savePath = Path.Combine(_testDirectory, "mytestgame.save");

            // ▼▼▼ JAVÍTÁS ITT ▼▼▼
            // Létre kell hoznunk egy IHighScoreManager-t, amit átadhatunk a GameModel-nek.
            var highScoreManager = new HighScoreManager(_testDirectory);
            var originalModel = new GameModel(800, 600, highScoreManager);
            // ▲▲▲ JAVÍTÁS ITT ▲▲▲

            var asteroids = new List<Asteroid>
            {
                new Asteroid(100, 200, 600, 30, 5),
                new Asteroid(150, 250, 600, 50, 2)
            };
            originalModel.SetGameState(1234, TimeSpan.FromSeconds(45), 410, asteroids);

            // Act
            persistence.SaveGame(savePath, originalModel);
            GameData loadedData = persistence.LoadGame(savePath);

            // Assert
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