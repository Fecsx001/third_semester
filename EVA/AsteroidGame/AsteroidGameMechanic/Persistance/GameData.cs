namespace AsteroidGameMechanic.Persistance;

public class GameData
{
    public int Score { get; set; }
    public TimeSpan GameTime { get; set; }
    public int SpaceshipX { get; set; }
    public List<AsteroidData> Asteroids { get; set; } = new List<AsteroidData>();
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
}