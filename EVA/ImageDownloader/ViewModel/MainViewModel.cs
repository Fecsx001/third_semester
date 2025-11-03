using ELTE.ImageDownloader.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ELTE.ImageDownloader.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private WebPage? _model;

        private bool _isDownloading;

        public bool IsDownloading
        {
            get { return _isDownloading; }
            set {
                _isDownloading = value;
                // OnPropertyChanged("IsDownloading");
                // OnPropertyChanged(nameof(IsDownloading));
                OnPropertyChanged();
                OnPropertyChanged(nameof(DownloadButtonLabel));
            }
        }
        
        public string DownloadButtonLabel => IsDownloading ? "Betöltés megszakítása" : "Letöltés";

        private float _progress;

        public float Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged(); }
        }


        public ObservableCollection<BitmapImage> Images { get; } = [];

        public DelegateCommand DownloadCommand { get; }
        public EventHandler<BitmapImage>? ImageSelected; 
        
        public void CancelLoad()
        {
            _model?.CancelLoad();
        }

        public MainViewModel()
        {
            DownloadCommand = new DelegateCommand(async (param) =>
                {
                    if (IsDownloading)
                    {
                        CancelLoad();
                    }
                    else
                    {
                        if (param is string url)
                            await LoadAsync(new Uri(url));
                    }
                    
                },
                _ => !IsDownloading
            );
            ImageSelectedCommand = new DelegateCommand((param) =>
            {
                if (param is BitmapImage image)
                {
                    ImageSelected?.Invoke(this, image);
                }
            });
        }

        private async Task LoadAsync(Uri uri)
        {
            Images.Clear();
            _model = new WebPage(uri);
            _model.ImageLoaded += OnImageLoaded;
            _model.LoadProgress += OnLoadProgress;

            IsDownloading = true;
            await _model.LoadImagesAsync();
            IsDownloading = false;
        }

        private void OnLoadProgress(object? sender, int e)
        {
            Progress = e;
        }

        private void OnImageLoaded(object? sender, WebImage e)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(e.Data);
            bitmap.EndInit();
            Images.Add(bitmap);
        }
    }
}