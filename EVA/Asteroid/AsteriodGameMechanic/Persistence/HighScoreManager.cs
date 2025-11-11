using System;
using System.IO;
using System.Text;

namespace AsteriodGameMechanic.Persistence
{
    public class HighScoreManager : IHighScoreManager
    {
        private readonly string _saveDirectory;
        private readonly string _highScoreFile;

        public HighScoreManager(string gameRootPath)
        {
            _saveDirectory = Path.Combine(gameRootPath, "SaveData");
            _highScoreFile = Path.Combine(_saveDirectory, "highscore.txt");
            
            Directory.CreateDirectory(_saveDirectory);
        }

        public int LoadHighScore()
        {
            try
            {
                if (File.Exists(_highScoreFile))
                {
                    string content = File.ReadAllText(_highScoreFile, Encoding.UTF8);
                    if (int.TryParse(content.Trim(), out int highScore))
                    {
                        return highScore;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading high score: {ex.Message}");
            }
            
            return 0;
        }

        public void SaveHighScore(int score)
        {
            try
            {
                int currentHighScore = LoadHighScore();
                
                if (score > currentHighScore)
                {
                    File.WriteAllText(_highScoreFile, score.ToString(), Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving high score: {ex.Message}");
            }
        }

        public string GetSaveDirectory()
        {
            return _saveDirectory;
        }
    }
}