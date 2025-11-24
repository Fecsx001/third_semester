using ImageDownloaderAvalonia.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace ImageDownloaderAvalonia.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private WebPage? _model;
        private bool _isDownloading;
        private float _progress;

        public bool IsDownloading
        {
            get => _isDownloading;
            private set
            {
                _isDownloading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownloadButtonLabel));
            }
        }

        public string DownloadButtonLabel
        {
            get => _isDownloading ? "Letöltés megszakítása" : "Képek betöltése";
        }

        public float Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Bitmap> Images { get; set; }

        public DelegateCommand DownloadCommand { get; }
        public DelegateCommand ImageSelectedCommand { get; }

        public event EventHandler<Bitmap>? ImageSelected;
        public event EventHandler<string>? ErrorOccurred;

        public MainViewModel()
        {
            Images = new ObservableCollection<Bitmap>();

            DownloadCommand = new DelegateCommand(param =>
            {
                if (!_isDownloading)
                {
                    _ = LoadAsync(new Uri(param?.ToString() ?? string.Empty));
                }
                else
                {
                    CancelLoad();
                }
            });

            ImageSelectedCommand = new DelegateCommand(param =>
            {
                if (param is Bitmap bitmap)
                    ImageSelected?.Invoke(this, bitmap);
            });
        }

        public void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }

        public async Task LoadAsync(Uri url)
        {
            try
            {
                IsDownloading = true;
                Images.Clear();

                _model = new WebPage(url);
                _model.ImageLoaded += OnImageLoaded;
                _model.LoadProgress += OnLoadProgress;
                await _model.LoadImagesAsync();

                IsDownloading = false;
            }
            catch (Exception e)
            {
                ErrorOccurred?.Invoke(this, e.Message);
            }

            _isDownloading = false;
        }

        private static readonly string[] SupportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private void OnImageLoaded(object? sender, WebImage e)
        {
            
            if (!SupportedExtensions.Contains(Path.GetExtension(e.Url.LocalPath))) return;
            
            var bitmapImage = new Bitmap(new MemoryStream(e.Data));

            Images.Add(bitmapImage);
        }

        private void OnLoadProgress(object? sender, int e)
        {
            Progress = e;
        }

        private void CancelLoad()
        {
            if (IsDownloading)
            {
                _model?.CancelLoad();
            }
        }
    }
}
