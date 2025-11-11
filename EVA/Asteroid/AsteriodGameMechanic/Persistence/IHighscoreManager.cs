using System;

namespace AsteriodGameMechanic.Persistence
{
    public interface IHighScoreManager
    {
        int LoadHighScore();
        void SaveHighScore(int score);
        string GetSaveDirectory();
    }
}