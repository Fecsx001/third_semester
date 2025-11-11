using System;

namespace PingPongGame.Model
{
    public class PingPongModel
    {
        private bool _isStarted;

        private readonly double _height;
        private readonly double _width;

        private readonly Element _ball;
        private readonly Element _pad;

        /// <summary>
        /// Event to notify that the ball has a new target position
        /// </summary>
        public event EventHandler<NextPositionEventArgs>? BallMoveToNextPosition;

        /// <summary>
        /// Event to notify that the pad has a new target position
        /// </summary>
        public event EventHandler<NextPositionEventArgs>? PadMoveToNextPosition;

        /// <summary>
        /// Event to notify that the game is over
        /// </summary>
        public event EventHandler? GameOver;

        /// <summary>
        /// Initialize
        /// </summary>
        public PingPongModel(double width, double height, Element ball, Element pad)
        {
            _height = height;
            _width = width;

            _ball = ball;
            _pad = pad;
        }

        /// <summary>
        /// Start new game
        /// </summary>
        /// <remarks>
        /// Sets a new random target position for the ball
        /// </remarks>
        public void StartGame()
        {
            _isStarted = true;

            Random random = new();

            //Start the ball towards a random direction
            int[] possibleLeft = new[] { 0, (int)_width };
            int[] possibleTop = new[] { 0, (int)_height };
            OnBallNextPosition(possibleLeft[random.Next(0, 2)], possibleTop[random.Next(0, 2)]);
        }

        /// <summary>
        /// Set a new target position for the pad
        /// </summary>
        public void MovePadToDirection(Direction direction)
        {
            if (!_isStarted)
                return;

            double nextLeft;
            switch (direction)
            {
                case Direction.Left:
                    nextLeft = _pad.Left - 100 < 0 ? 0 : _pad.Left - 100;
                    OnPadNextPosition(nextLeft, _pad.Top);
                    break;
                case Direction.Right:
                    nextLeft = _pad.Right + 100 > _width ? _width - _pad.Width : _pad.Left + 100;
                    OnPadNextPosition(nextLeft, _pad.Top);
                    break;
            }
        }

        /// <summary>
        /// Update the position of the pad
        /// </summary>
        public void MovePad(double left, double top)
        {
            if (!_isStarted)
                return;

            _pad.Move(left, top);
        }

        /// <summary>
        /// Update the position of the ball
        /// </summary>
        /// <remarks>
        /// If the ball reached the wall or the pad, set a new target position
        /// If the ball went over the pad, game over
        /// </remarks>
        public void MoveBall(double left, double top)
        {
            if (!_isStarted)
                return;

            _ball.Move(left, top);

            Direction? padCollision = _ball.IsCollisionToOther(_pad);

            if (padCollision != null)
            {
                switch (padCollision)
                {
                    case Direction.Left:
                        OnBallNextPosition(_ball.Left - 200, 0, true);
                        break;
                    case Direction.Right:
                        OnBallNextPosition(_ball.Left + 200, 0, true);
                        break;
                    default:
                        break;
                }
                return;
            }
            else if (_ball.IsBelow(_pad))
            {
                OnGameOver();
                return;
            }

            Direction? wallCollision = _ball.IsCollisionToWall(_width, _height);

            if (wallCollision != null)
            {
                switch (wallCollision)
                {
                    case Direction.Left:
                        OnBallNextPosition(_width - _ball.Width, _height);
                        break;
                    case Direction.Up:
                        OnBallNextPosition(_ball.Left, _height);
                        break;
                    case Direction.Right:
                        OnBallNextPosition(0, _height);
                        break;
                    default:
                        break;
                }
                return;
            }
        }

        private void OnPadNextPosition(double left, double top)
        {
            PadMoveToNextPosition?.Invoke(this, new NextPositionEventArgs(left, top));
        }

        private void OnBallNextPosition(double left, double top, bool wasCollision = false)
        {
            BallMoveToNextPosition?.Invoke(this, new NextPositionEventArgs(left, top, wasCollision));
        }

        private void OnGameOver()
        {
            _isStarted = false;
            GameOver?.Invoke(this, EventArgs.Empty);
        }

    }
}
