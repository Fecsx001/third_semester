using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ImageDownloaderAvalonia.ViewModels;
using ImageDownloaderAvalonia.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace ImageDownloaderAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private TopLevel? TopLevel
    {
        get
        {
            return ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var mainViewModel = new MainViewModel();
        mainViewModel.ImageSelected += ImageSelected;
        mainViewModel.ErrorOccurred += OnErrorOccured;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ImageSelected(object? sender, Bitmap e)
    {
        ImageViewModel viewModel = new ImageViewModel(e);
        viewModel.SaveImage += SaveImage;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ImageWindow window = new ImageWindow()
            {
                DataContext = viewModel
            };
            window.Show(desktop.MainWindow!); // owner megadása paraméterként
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var mainView = singleViewPlatform.MainView;
            viewModel.Close += (sender, e) =>
            {
                singleViewPlatform.MainView = mainView;
            }; // visszaállítjuk a fő nézetet a kép megjelenítő nézet bezárásakor

            singleViewPlatform.MainView = new ImageView
            {
                DataContext = viewModel
            };
        }
    }

    private async void OnErrorOccured(object? sender, string e)
    {
        await MessageBoxManager.GetMessageBoxStandard("Image Downloader", e, ButtonEnum.Ok, Icon.Error).ShowAsync();
    }

    private async void SaveImage(object? sender, Bitmap e)
    {
        if (TopLevel == null)
            return;

        var file = await TopLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save image",
            SuggestedFileName = "download.png",
            FileTypeChoices = new [] {FilePickerFileTypes.ImagePng }
        });

        if (file != null)
        {
            using var stream = new MemoryStream();
            e.Save(stream); // írjuk adatfolyamba a bitmap-et
            stream.Seek(0, SeekOrigin.Begin); // ugorjunk vissza az adatfolyam elejére

            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(stream);
            using var fileStream = await file.OpenWriteAsync();
            await image.SaveAsync(fileStream, new PngEncoder());
            // mentsük PNG-ként a képet az ImageSharp osztálykönyvtár használatával
        }
    }
}