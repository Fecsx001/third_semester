using PingPongGame.Model;
using System;
using System.Windows;

namespace PingPongGame.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PingPongModel _model;

        private bool _isCollision;

        /// <summary>
        /// Start position of the ball
        /// </summary>
        public Thickness BallStartPosition { get; private set; }
        /// <summary>
        /// Start position of the pad
        /// </summary>
        public Thickness PadStartPosition { get; private set; }

        /// <summary>
        /// Size of the grid
        /// </summary>
        public Size GridSize { get; private set; }
        /// <summary>
        /// Size of the ball
        /// </summary>
        public Size BallSize { get; private set; }
        /// <summary>
        /// Size of the pad
        /// </summary>
        public Size PadSize { get; private set; }

        /// <summary>
        /// True if the ball hit the pad
        /// </summary>
        public bool IsCollision
        {
            get => _isCollision;
            private set
            {
                _isCollision = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Event to notify that the ball has a new target position
        /// </summary>
        public event EventHandler<Thickness>? BallNextPosition;
        /// <summary>
        /// Event to notify that the pad has a new target position
        /// </summary>
        public event EventHandler<Thickness>? PadNextPosition;
        /// <summary>
        /// Event to notify that the game is over
        /// </summary>
        public event EventHandler? GameOver;

        /// <summary>
        /// Command to handle right or left key press
        /// </summary>
        public DelegateCommand MovePadCommand { get; set; }
        /// <summary>
        /// Command to start a new game
        /// </summary>
        public DelegateCommand StartGameCommand { get; set; }

        /// <summary>
        /// Initialize
        /// </summary>
        public MainViewModel()
        {
            GridSize = new Size(800, 450);
            BallSize = new Size(40, 40);
            PadSize = new Size(120, 10);

            StartGameCommand = new DelegateCommand(_ => StartGame());

            MovePadCommand = new DelegateCommand(param =>
            {
                if (param is string stringParam)
                {
                    bool success = Enum.TryParse(stringParam, out Direction direction);
                    if (!success)
                        return;
                   
                    _model?.MovePadToDirection(direction);
                }
            });

            BallStartPosition = new Thickness(GridSize.Width / 2 - BallSize.Width / 2, GridSize.Height / 2, 0, 0);
            PadStartPosition = new Thickness(GridSize.Width / 2 - PadSize.Width / 2, GridSize.Height - 100, 0, 0);

            _model = new PingPongModel(GridSize.Width, GridSize.Height,
            new Element(BallStartPosition.Left, BallStartPosition.Top, BallSize.Width, BallSize.Height),
            new Element(PadStartPosition.Left, PadStartPosition.Top, PadSize.Height, PadSize.Width));

            _model.BallMoveToNextPosition += OnBallNextPosition;
            _model.PadMoveToNextPosition += OnPadNextPosition;
            _model.GameOver += (o, e) => GameOver?.Invoke(this, e);

        }

        /// <summary>
        /// Start new game
        /// </summary>
        public void StartGame()
        {
            _model.StartGame();
        }

        /// <summary>
        /// Update the position of the ball
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void MoveBall(double left, double top)
        {
            IsCollision = false;
            _model.MoveBall(left, top);
        }

        /// <summary>
        /// Update the position of the pad
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void MovePad(double left, double top)
        {
            _model.MovePad(left, top);
        }


        /// <summary>
        /// Handle the next target position of the ball
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBallNextPosition(object? sender, NextPositionEventArgs e)
        {
            if (e.WasCollision)
            {
                IsCollision = true;
            }

            Thickness nextPosition = new(e.Left, e.Top, 0, 0);
            BallNextPosition?.Invoke(this, nextPosition);
        }


        /// <summary>
        /// Handle the next target position of the pad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPadNextPosition(object? sender, NextPositionEventArgs e)
        {
            Thickness nextPosition = new(e.Left, e.Top, 0, 0);
            PadNextPosition?.Invoke(this, nextPosition);
        }
    }
}
