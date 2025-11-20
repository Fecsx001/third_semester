using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageDownloaderAvalonia.Model
{
    public class WebImage
    {
        public Uri Url { get; private set; }

        public byte[] Data { get; private set; }

        protected WebImage(Uri url, byte[] data)
        {
            Url = url;
            Data = data;
        }

        public static async Task<WebImage> DownloadAsync(Uri url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (!url.IsAbsoluteUri)
                throw new ArgumentException("URL must be absolute.", nameof(url));

            HttpClient client = new HttpClient();
            byte[] data = await client.GetByteArrayAsync(url);

            return new WebImage(url, data);
        }
    }
}
