using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AsteroidGameAvalonia.Persistance
{
    public class GamePersistence : IGamePersistence
    {
        public async Task SaveGameAsync(string filePath, GameData gameData)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("filePath is null or empty", nameof(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteLineAsync(gameData.Score.ToString()).ConfigureAwait(false);
                await writer.WriteLineAsync(gameData.GameTime.TotalSeconds.ToString()).ConfigureAwait(false);
                await writer.WriteLineAsync(gameData.ScreenWidth.ToString()).ConfigureAwait(false);
                await writer.WriteLineAsync(gameData.ScreenHeight.ToString()).ConfigureAwait(false);
                await writer.WriteLineAsync(gameData.SpaceshipX.ToString()).ConfigureAwait(false);
                await writer.WriteLineAsync(gameData.Asteroids.Count.ToString()).ConfigureAwait(false);

                foreach (var asteroid in gameData.Asteroids)
                {
                    await writer.WriteLineAsync(asteroid.X.ToString()).ConfigureAwait(false);
                    await writer.WriteLineAsync(asteroid.Y.ToString()).ConfigureAwait(false);
                    await writer.WriteLineAsync(asteroid.BaseSize.ToString()).ConfigureAwait(false);
                    await writer.WriteLineAsync(asteroid.Speed.ToString()).ConfigureAwait(false);
                }
            }
        }

        public async Task<GameData> LoadGameAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException("filePath is null or empty", nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException("Save file not found.", filePath);

            string[] lines;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var reader = new StreamReader(stream))
            {
                var content = await reader.ReadToEndAsync().ConfigureAwait(false);
                lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return await Task.Run(() =>
            {
                int idx = 0;
                GameData gameData = new GameData();
                try
                {
                    gameData.Score = int.Parse(lines[idx++]);
                    gameData.GameTime = TimeSpan.FromSeconds(double.Parse(lines[idx++]));
                    gameData.ScreenWidth = int.Parse(lines[idx++]);
                    gameData.ScreenHeight = int.Parse(lines[idx++]);
                    gameData.SpaceshipX = int.Parse(lines[idx++]);
                    gameData.Asteroids = new List<AsteroidData>();

                    int asteroidCount = int.Parse(lines[idx++]);
                    for (int i = 0; i < asteroidCount; i++)
                    {
                        var ad = new AsteroidData
                        {
                            X = int.Parse(lines[idx++]),
                            Y = int.Parse(lines[idx++]),
                            BaseSize = int.Parse(lines[idx++]),
                            Speed = int.Parse(lines[idx++])
                        };
                        gameData.Asteroids.Add(ad);
                    }

                    return gameData;
                }
                catch (Exception ex) when (ex is IndexOutOfRangeException || ex is FormatException)
                {
                    throw new IOException("Save file is corrupt or in an unexpected format.", ex);
                }
            }).ConfigureAwait(false);
        }
    }
}