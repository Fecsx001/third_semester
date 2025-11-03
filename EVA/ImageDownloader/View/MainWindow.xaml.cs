using ELTE.ImageDownloader.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ELTE.ImageDownloader.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private WebPage? _model;

        private bool _isDownloading;


        public MainWindow()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }

        private async void StartStopLoad(object sender, RoutedEventArgs e)
        {
            if (!_isDownloading)
            {
                _isDownloading = true;

                loadProgressBar.Value = 0;
                imageCountValue.Text = 0.ToString();
                loadProgressBar.Visibility = Visibility.Visible;
                loadButton.Content = "Letöltés megszakítása";
                picturePanel.Children.Clear();

                await StartLoadAsync();

                loadButton.Content = "Képek letöltése";
                loadProgressBar.Visibility = Visibility.Hidden;
                _isDownloading = false;
            }
            else
            {
                CancelLoad();
            }
        }

        private async Task StartLoadAsync()
        {
            _model = new WebPage(new Uri(urlTextBox.Text));
            _model.ImageLoaded += ImageLoaded;
            _model.LoadProgress += LoadProgress;
            await _model.LoadImagesAsync();
        }

        private void CancelLoad()
        {
            _model?.CancelLoad();
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
            image.MouseDown += ShowImage;

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

        private void ShowImage(object sender, MouseButtonEventArgs e)
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
