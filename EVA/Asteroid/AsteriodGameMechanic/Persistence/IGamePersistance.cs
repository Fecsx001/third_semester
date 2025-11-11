using AsteriodGameMechanic.Model;
using AsteriodGameMechanic.Persistence;
using System;
using System.Collections.Generic;

namespace AsteriodGameMechanic.Persistence
{
    public interface IGamePersistence
    {
        void SaveGame(string filePath, GameModel gameModel);
        GameData LoadGame(string filePath);
    }
}