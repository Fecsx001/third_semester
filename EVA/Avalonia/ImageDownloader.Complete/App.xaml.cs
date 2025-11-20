using ELTE.ImageDownloader.View;
using ELTE.ImageDownloader.ViewModel;
using Microsoft.Win32;
using System.IO;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ELTE.ImageDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel? _mainViewModel;
        private MainWindow? _mainWindow;

        public App()
        {
            Startup += OnStartup;
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel();
            _mainViewModel.ImageSelected += ImageSelected;

            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
            _mainWindow.Show();
        }

        private void ImageSelected(object? sender, BitmapImage e)
        {
            ImageViewModel viewModel = new ImageViewModel(e);
            viewModel.SaveImage += SaveImage;

            ImageWindow window = new ImageWindow
            {
                Owner = _mainWindow,
                DataContext = viewModel
            };
            window.Show();
        }

        private void SaveImage(object? sender, BitmapImage e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                RestoreDirectory = true,
                Filter = "PNG files (*.png)|*.png"
            };

            if (saveDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveDialog.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(e));
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
