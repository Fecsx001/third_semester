namespace AsteriodGameMechanic.Model
{
    public class Asteroid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private readonly int _speed;
        private int _baseSize;

        public Asteroid(int x, int y, int screenHeight, int baseSize, int speed)
        {
            X = x;
            Y = y;
            _baseSize = baseSize;
            _speed = speed;
            
            Random rand = new Random();
            int sizeVariation = (int)(_baseSize * (rand.NextDouble() * 0.4 - 0.2));
            Width = _baseSize + sizeVariation;
            Height = _baseSize + sizeVariation;
        }

        public void Move()
        {
            Y += _speed;
        }
    }
}