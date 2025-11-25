namespace AsteroidGameAvalonia.Persistance
{
    using System.Threading.Tasks;

    public interface IGamePersistence
    {
        Task SaveGameAsync(string filePath, GameData gameData);
        Task<GameData> LoadGameAsync(string filePath);
    }
}