namespace PingPongGame.Model
{
    public class Element
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Right => Left + Width;
        public double Bottom => Top + Height;

        /// <summary>
        /// Initialize the element
        /// </summary>
        public Element(double left, double top, double height, double width)
        {
            Top = top;
            Left = left;
            Height = height;
            Width = width;
        }

        /// <summary>
        /// Update the coordinates
        /// </summary>
        public void Move(double left, double top)
        {
            Top = top;
            Left = left;
        }

        /// <summary>
        /// Check if element has reached the wall
        /// </summary>
        public Direction? IsCollisionToWall(double width, double height)
        {
            if (Right >= width)
                return Direction.Right;

            if (Left <= 0)
                return Direction.Left;

            if (Top <= 0)
                return Direction.Up;

            if (Bottom >= height)
                return Direction.Down;

            return null;
        }

        /// <summary>
        /// Check if element has collision
        /// </summary>
        public Direction? IsCollisionToOther(Element element)
        {
            if (IsLeftCollision(element))
                return Direction.Left;

            if (IsRightCollision(element))
                return Direction.Right;

            return null;
        }

        private bool IsLeftCollision(Element other)
        {
            return Bottom >= other.Top && Top <= other.Top &&
                   Right >= other.Left &&
                   Left <= other.Left + other.Width / 2;
        }

        private bool IsRightCollision(Element other)
        {
            return Bottom >= other.Top && Top <= other.Top &&
                   Right >= other.Left + other.Width / 2 &&
                   Left <= other.Right;
        }

        public bool IsBelow(Element other)
        {
            return Top >= other.Bottom;
        }

    }
}
