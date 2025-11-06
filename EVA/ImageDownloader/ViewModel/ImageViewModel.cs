using System;
using System.Windows.Media.Imaging;

namespace ELTE.ImageDownloader.ViewModel;

public class ImageViewModel
{
    public BitmapImage Image { get; }
    
    public DelegateCommand SaveImageCommand { get; }
    public EventHandler<BitmapImage>? SaveImage;

    public ImageViewModel(BitmapImage image)
    {
        Image = image;
        SaveImageCommand = new DelegateCommand(_ => SaveImage?.Invoke(this, Image));
    }
}