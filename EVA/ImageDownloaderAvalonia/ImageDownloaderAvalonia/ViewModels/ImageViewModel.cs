using Avalonia.Media.Imaging;
using System;

namespace ImageDownloaderAvalonia.ViewModels;

public class ImageViewModel : ViewModelBase
{
    public Bitmap Image { get; private set; }

    public DelegateCommand SaveImageCommand { get; private set; }
    public DelegateCommand CloseCommand { get; private set; }

    public event EventHandler<Bitmap>? SaveImage;

    public event EventHandler? Close;

    public ImageViewModel(Bitmap image)
    {
        Image = image;

        SaveImageCommand = new DelegateCommand(_ =>
        {
            SaveImage?.Invoke(this, Image);
        });

        CloseCommand = new DelegateCommand(_ =>
        {
            Close?.Invoke(this, EventArgs.Empty);
        });
    }
}