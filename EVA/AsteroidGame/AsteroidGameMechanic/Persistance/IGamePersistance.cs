using System;
using System.Collections.Generic;

namespace AsteroidGameMechanic.Persistance
{
    public interface IGamePersistence
    {
        void SaveGame(string filePath, GameData gameData);
        GameData LoadGame(string filePath);
    }
}