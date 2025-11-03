using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ELTE.ImageDownloader.Model
{
    public class WebPage : IDisposable
    {
        private List<WebImage> _images;
        private CancellationTokenSource _cancelSource;
        private CancellationToken _cancelToken;
        public Uri BaseUrl { get; private set; }

        public int ImageCount
        {
            get { return _images.Count; }
        }

        public ICollection<WebImage> Images
        {
            get { return _images.AsReadOnly(); }
        }

        public event EventHandler<WebImage>? ImageLoaded;

        public event EventHandler<int>? LoadProgress; 

        public WebPage(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (!url.IsAbsoluteUri)
                throw new ArgumentException("URL must be absolute.", nameof(url));

            BaseUrl = url;
            _images = new List<WebImage>();

            _cancelSource = new CancellationTokenSource();
            _cancelToken = _cancelSource.Token;
        }

        public async Task LoadImagesAsync()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(BaseUrl, _cancelToken);
            var content = await response.Content.ReadAsStringAsync();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(content);
            var nodes = doc.DocumentNode.SelectNodes("//img");

            _images.Clear();
            int counter = 0;

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (_cancelToken.IsCancellationRequested)
                        break;

                    ++counter;
                    LoadProgress?.Invoke(this, counter * 100 / nodes.Count);

                    if (!node.Attributes.Contains("src"))
                        continue;

                    Uri imageUrl = new Uri(BaseUrl, node.Attributes["src"].Value);
                    try
                    {
                        var image = await WebImage.DownloadAsync(imageUrl);
                        _images.Add(image);
                        ImageLoaded?.Invoke(this, image);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        public void CancelLoad()
        {
            _cancelSource.Cancel();
        }

        public void Dispose()
        {
            _cancelSource.Dispose();
        }
    }
}
