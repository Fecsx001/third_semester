using System;

namespace AsteroidGameAvalonia.Persistance
{
    public interface IHighScoreManager
    {
        int LoadHighScore();
        void SaveHighScore(int score);
        string GetSaveDirectory();
    }
}