using System;
using Avalonia.Media.Imaging;

namespace ImageDownloaderAvalonia.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public Bitmap Image { get; private set; }

        public DelegateCommand SaveImageCommand { get; private set; }

        public event EventHandler<Bitmap>? SaveImage;

        public ImageViewModel(Bitmap image)
        {
            Image = image;

            SaveImageCommand = new DelegateCommand(_ =>
            {
                SaveImage?.Invoke(this, Image);
            });
        }
    }
}
