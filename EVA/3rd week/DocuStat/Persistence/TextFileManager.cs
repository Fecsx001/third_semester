using System;
using System.IO;

namespace ELTE.DocuStat.Persistence
{
    public class TextFileManager : IFileManager
    {
        private readonly string _path;

        public TextFileManager(string path)
        {
            _path = path;
        }

        public string Load()
        {
            try
            {
                return File.ReadAllText(_path);
            }
            catch (Exception ex)
            {
                throw new FileManagerException(ex.Message, ex);
            }
        }
    }
}