namespace AsteroidGame.Model
{
    public class Spaceship
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; } = 60;
        public int Height { get; private set; } = 30;
        private int _screenWidth;
        private int _speed = 10;

        public Spaceship(int x, int y, int screenWidth)
        {
            X = x;
            Y = y;
            _screenWidth = screenWidth;
        }

        public void MoveLeft()
        {
            X = System.Math.Max(0, X - _speed);
        }

        public void MoveRight()
        {
            X = System.Math.Min(_screenWidth - Width, X + _speed);
        }
    }
}