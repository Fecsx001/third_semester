using System;

namespace PingPongGame.Model
{
    public class NextPositionEventArgs : EventArgs
    {
        public double Left { get; set; }

        public double Top { get; set; }

        /// <summary>
        /// True if the element has collided with an other element
        /// </summary>
        public bool WasCollision { get; set; }

        /// <summary>
        /// Initialize
        /// </summary>
        public NextPositionEventArgs(double left, double top, bool collision = false)
        {
            Top = top;
            Left = left;
            WasCollision = collision;
        }
    }
}
