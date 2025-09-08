using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EvaWeek1
{
    internal class DocumentStatistics
    {
        private readonly string _filePath;

        public string FileContent { get; private set; }

        public IDictionary<string, int> DistinctWordCount { get; private set; }

        public DocumentStatistics(string filePath)
        {
            _filePath = filePath;
        }

        public void Load()
        {
            FileContent = File.ReadAllText(_filePath);
        }

        public Dictionary<string, int> ComputeDistinctWords(string text)
        {
            DistinctWordCount = new Dictionary<string, int>();

            string[] words = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            foreach (string rawWord in words)
            {
                string word = rawWord.Trim().ToLower();

                word = word.TrimStart(c => !Char.IsLetter(c))
                           .TrimEnd(c => !Char.IsLetter(c));

                if (!string.IsNullOrEmpty(word))
                {
                    if (DistinctWordCount.ContainsKey(word))
                        DistinctWordCount[word]++;
                    else
                        DistinctWordCount[word] = 1;
                }
            }

            return DistinctWordCount.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }

    internal static class StringExtensions
    {
        public static string TrimStart(this string str, Func<char, bool> predicate)
        {
            int i = 0;
            while (i < str.Length && predicate(str[i])) i++;
            return str.Substring(i);
        }

        public static string TrimEnd(this string str, Func<char, bool> predicate)
        {
            int i = str.Length - 1;
            while (i >= 0 && predicate(str[i])) i--;
            return str.Substring(0, i + 1);
        }
    }
}
