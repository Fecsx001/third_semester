using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PingPongGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<Thickness>? BallLayoutUpdated;
        public event EventHandler<Thickness>? PadLayoutUpdated;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartBallAnimation(Thickness nextPosition)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = Ellipse.Margin,
                To = nextPosition,
                Duration = new Duration(TimeSpan.FromMilliseconds(5)),
                SpeedRatio = 1 / TravelDistance(Ellipse.Margin, nextPosition) //Speed depends on the distance to travel
            };

            Ellipse.BeginAnimation(Ellipse.MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        public void StartPadAnimation(Thickness nextPosition)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = Rectangle.Margin,
                To = nextPosition,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            };

            Rectangle.BeginAnimation(Rectangle.MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        private void OnBallLayoutUpdated(object? sender, EventArgs e)
        {
            BallLayoutUpdated?.Invoke(this, Ellipse.Margin);
        }

        private void OnPadLayoutUpdated(object? sender, EventArgs e)
        {
            PadLayoutUpdated?.Invoke(this, Rectangle.Margin);
        }

        private static double TravelDistance(Thickness currentPosition, Thickness nextPosition)
        {
            return Math.Sqrt(Math.Pow(nextPosition.Left - currentPosition.Left, 2) +
                             Math.Pow(nextPosition.Top - currentPosition.Top, 2));
        }
    }
}