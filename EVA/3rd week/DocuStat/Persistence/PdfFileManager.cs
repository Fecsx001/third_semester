using System;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace ELTE.DocuStat.Persistence
{
    public class PdfFileManager : IFileManager
    {
        private readonly string _path;

        public PdfFileManager(string path)
        {
            _path = path;
        }

        public string Load()
        {
            try
            {
                using var reader = new PdfReader(_path);
                using var document = new PdfDocument(reader);

                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    var page = document.GetPage(i);
                    text.Append(PdfTextExtractor.GetTextFromPage(page));
                }

                return text.ToString();
            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
            }
        }
    }
}