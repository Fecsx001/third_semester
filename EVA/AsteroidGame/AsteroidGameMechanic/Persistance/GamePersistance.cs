using System.IO;

namespace AsteroidGameMechanic.Persistance
{
    public class GamePersistence : IGamePersistence
    {
        public void SaveGame(string filePath, GameData gameData)
        {

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(gameData.Score);
                writer.WriteLine(gameData.GameTime.TotalSeconds);
                writer.WriteLine(gameData.ScreenWidth);
                writer.WriteLine(gameData.ScreenHeight);
                writer.WriteLine(gameData.SpaceshipX);
                writer.WriteLine(gameData.Asteroids.Count);
                
                foreach (var asteroid in gameData.Asteroids)
                {
                    writer.WriteLine(asteroid.X);
                    writer.WriteLine(asteroid.Y);
                    writer.WriteLine(asteroid.BaseSize);
                    writer.WriteLine(asteroid.Speed);
                }
            }
        }

        public GameData LoadGame(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                var gameData = new GameData
                {
                    Score = int.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Score is missing.")),
                    GameTime = TimeSpan.FromSeconds(double.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Score is missing."))),
                    ScreenWidth = int.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Score is missing.")),
                    ScreenHeight = int.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Score is missing.")),
                    SpaceshipX = int.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Score is missing.")),
                    Asteroids = new List<AsteroidData>()
                };

                int asteroidCount = int.Parse(reader.ReadLine() ?? throw new IOException("Save file is corrupt: Asteroid count is missing."));
                for (int i = 0; i < asteroidCount; i++)
                {
                    gameData.Asteroids.Add(new AsteroidData
                    {
                        X = int.Parse(reader.ReadLine() ?? throw new IOException($"Save file is corrupt: Asteroid {i} X missing.")),
                        Y = int.Parse(reader.ReadLine() ?? throw new IOException($"Save file is corrupt: Asteroid {i} Y missing.")),
                        BaseSize = int.Parse(reader.ReadLine() ?? throw new IOException($"Save file is corrupt: Asteroid {i} BaseSize missing.")),
                        Speed = int.Parse(reader.ReadLine() ?? throw new IOException($"Save file is corrupt: Asteroid {i} Speed missing."))
                    });
                }

                return gameData;
            }
        }
    }
}