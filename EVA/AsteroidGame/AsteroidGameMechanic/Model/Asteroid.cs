namespace AsteroidGameMechanic.Model
{
    public class Asteroid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int BaseSize { get; private set; }
        public int Speed { get; private set; }

        public Asteroid(int x, int y, int screenHeight, int baseSize, int speed)
        {
            X = x;
            Y = y;
            BaseSize = baseSize;
            Speed = speed;
            
            Random rand = new Random();
            int sizeVariation = (int)(BaseSize * (rand.NextDouble() * 0.4 - 0.2));
            Width = BaseSize + sizeVariation;
            Height = BaseSize + sizeVariation;
        }

        public void Move()
        {
            Y += Speed;
        }
    }
}