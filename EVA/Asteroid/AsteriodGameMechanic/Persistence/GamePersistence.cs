using AsteriodGameMechanic.Model;

namespace AsteriodGameMechanic.Persistence
{
    public class GamePersistence
    {
        public void SaveGame(string filePath, GameModel gameModel)
        {
            var gameData = new GameData
            {
                Score = gameModel.Score,
                GameTime = gameModel.GameTime,
                SpaceshipX = gameModel.Spaceship.X,
                ScreenWidth = gameModel.ScreenWidth,
                ScreenHeight = gameModel.ScreenHeight,
                Asteroids = new List<AsteroidData>()
            };

            foreach (var asteroid in gameModel.Asteroids)
            {
                int baseSize = (asteroid.Width + asteroid.Height) / 2;
                gameData.Asteroids.Add(new AsteroidData
                {
                    X = asteroid.X,
                    Y = asteroid.Y,
                    BaseSize = baseSize,
                    Speed = 5
                });
            }

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
                    Score = int.Parse(reader.ReadLine()),
                    GameTime = TimeSpan.FromSeconds(double.Parse(reader.ReadLine())),
                    ScreenWidth = int.Parse(reader.ReadLine()),
                    ScreenHeight = int.Parse(reader.ReadLine()),
                    SpaceshipX = int.Parse(reader.ReadLine()),
                    Asteroids = new List<AsteroidData>()
                };

                int asteroidCount = int.Parse(reader.ReadLine());
                for (int i = 0; i < asteroidCount; i++)
                {
                    gameData.Asteroids.Add(new AsteroidData
                    {
                        X = int.Parse(reader.ReadLine()),
                        Y = int.Parse(reader.ReadLine()),
                        BaseSize = int.Parse(reader.ReadLine()),
                        Speed = int.Parse(reader.ReadLine())
                    });
                }

                return gameData;
            }
        }
    }
}