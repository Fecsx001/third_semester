using ImageDownloader.Model;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebPage? _model;



        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartLoadAsync(object sender, RoutedEventArgs e)
        {
            loadProgressBar.Value = 0;
            imageCountValue.Text = 0.ToString();
            loadProgressBar.Visibility = Visibility.Visible;
            picturePanel.Children.Clear();

            _model = new WebPage(new Uri(urlTextBox.Text));
            _model.ImageLoaded += ImageLoaded;
            _model.LoadProgress += LoadProgress;
            await _model.LoadImagesAsync();

            loadProgressBar.Visibility = Visibility.Hidden;
        }

        private void ImageLoaded(object? sender, WebImage e)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(e.Data);
            bitmap.EndInit();

            var image = new Image
            {
                Source = bitmap,
                Width = 100,
                Height = 100
            };

            var border = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(2)
            };
            border.Child = image;

            picturePanel.Children.Add(border);
            imageCountValue.Text = _model?.ImageCount.ToString();
        }

        private void LoadProgress(object? sender, int e)
        {
            loadProgressBar.Value = e;
        }
        
        private void ShowImage(object sender, EventArgs e)
        {
            if(sender is Image image && image.Source is BitmapImage bitmap)
            {
                ImageWindow window = new ImageWindow(bitmap);
                window.Owner = this;
                window.Show();
            }
        }
    }
}