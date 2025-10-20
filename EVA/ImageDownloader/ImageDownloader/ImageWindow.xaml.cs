using System.Windows;
using Microsoft.Win32;

namespace ImageDownloader;

public partial class ImageWindow : Window
{
    public ImageWindow()
    {
        InitializeComponent();
        picture.Source = image;
    }

    private void Download(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveDialog = new SaveFileDialog();
        saveDialog.Filter = "PNG files (*.png)|*.png";
        saveDialog.InitialDirectory = Environment.GetFolderPath(
            Environment.SpecialFolder.MyPictures);
        saveDialog.RestoreDirectory = true;
        
        if (saveDialog.ShowDialog() == true)
        {
            using (var fileStream = saveDialog.OpenFile())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)picture.Source));
                encoder.Save(fileStream);
            }
        }
    }
    
    
}