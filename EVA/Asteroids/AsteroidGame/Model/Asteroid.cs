namespace AsteroidGame.Model
{
    public class Asteroid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; } = 40;
        public int Height { get; private set; } = 40;
        private int _screenHeight;
        private int _speed = 5;

        public Asteroid(int x, int y, int screenHeight)
        {
            X = x;
            Y = y;
            _screenHeight = screenHeight;
        }

        public void Move()
        {
            Y += _speed;
        }
    }
}