using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EvaWeek1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            do
            {
                Console.WriteLine("Enter a text file name (with extension):");
                filePath = Console.ReadLine() ?? "";
                filePath = $"../../../{filePath}";

            } while (Path.GetExtension(filePath) != ".txt" || !File.Exists(filePath));

            DocumentStatistics stat = new DocumentStatistics(filePath);

            try
            {
                stat.Load();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to load file. Message: {ex.Message}");
                return;
            }

            // Compute statistics from file
            stat.ComputeDistinctWords(stat.FileContent);

            // User filters
            int minOccurrence = ReadPositiveInt("Minimum occurrence: ");
            int minLength = ReadPositiveInt("Minimum length: ");

            Console.Write("Ignored words (separate with space): ");
            string[] ignoredWords = (Console.ReadLine() ?? "")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.ToLower())
                .ToArray();

            var pairs = stat.DistinctWordCount
                .Where(p => p.Value >= minOccurrence)
                .Where(p => p.Key.Length >= minLength)
                .Where(p => !ignoredWords.Contains(p.Key))
                .OrderByDescending(p => p.Value)
                .ThenBy(p => p.Key);

            Console.WriteLine("\nFiltered results:\n");
            foreach (var pair in pairs)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }

        private static int ReadPositiveInt(string msg)
        {
            int value = 0;
            bool success = false;
            do
            {
                Console.WriteLine(msg);
                string line = Console.ReadLine() ?? "";
                success = Int32.TryParse(line, out value) && value > 0;
            } while (!success);

            return value;
        }
    }
}