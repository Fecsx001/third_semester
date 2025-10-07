using System;

namespace AsteriodGameMechanic.Model
{
    public class Asteroid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private int _screenHeight;
        private int _speed;
        private int _baseSize;

        public Asteroid(int x, int y, int screenHeight, int baseSize, int speed)
        {
            X = x;
            Y = y;
            _screenHeight = screenHeight;
            _baseSize = baseSize;
            _speed = speed;
            
            // Add some random variation to size (±20%)
            Random rand = new Random();
            int sizeVariation = (int)(_baseSize * (rand.NextDouble() * 0.4 - 0.2));
            Width = _baseSize + sizeVariation;
            Height = _baseSize + sizeVariation;
        }

        public void Move()
        {
            Y += _speed;
        }

        public int GetBaseSize() => _baseSize;
        public int GetSpeed() => _speed;
    }
}