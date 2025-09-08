using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaWeek1
{
    internal class DocumentStatistics
    {
        private string _filePath;

        public string fileContent {get; private set;}

        public DocumentStatistics(string filePath)
        {
            _filePath = filePath;
        }

        public void Load()
        {
            fileContent = File.ReadAllText(_filePath);
        }
        
        
    }
}