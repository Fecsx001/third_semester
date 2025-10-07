using System;
using System.IO;
using System.Text;

namespace AsteriodGameMechanic.Persistence
{
    public class HighScoreManager
    {
        private string _saveDirectory;
        private string _highScoreFile;

        public HighScoreManager(string gameRootPath)
        {
            _saveDirectory = Path.Combine(gameRootPath, "SaveData");
            _highScoreFile = Path.Combine(_saveDirectory, "highscore.txt");
            
            // Ensure the SaveData directory exists
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
                // If there's any error reading the file, return 0
                System.Diagnostics.Debug.WriteLine($"Error loading high score: {ex.Message}");
            }
            
            return 0; // Default high score if file doesn't exist or is invalid
        }

        public void SaveHighScore(int score)
        {
            try
            {
                int currentHighScore = LoadHighScore();
                
                // Only save if the new score is higher
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

        public string GetHighScoreFilePath()
        {
            return _highScoreFile;
        }

        public string GetSaveDirectory()
        {
            return _saveDirectory;
        }
    }
}