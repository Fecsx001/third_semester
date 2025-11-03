using System.IO;

namespace ELTE.DocuStat.Persistence
{
    public static class FileManagerFactory
    {
        public static IFileManager? CreateForPath(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            return extension switch
            {
                ".txt" => new TextFileManager(path),
                ".pdf" => new PdfFileManager(path),
                _ => null
            };
        }
    }
}