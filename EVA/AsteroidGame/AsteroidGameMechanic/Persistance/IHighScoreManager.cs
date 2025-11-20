using System;

namespace AsteroidGameMechanic.Persistance
{
    public interface IHighScoreManager
    {
        int LoadHighScore();
        void SaveHighScore(int score);
        string GetSaveDirectory();
    }
}