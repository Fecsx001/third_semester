namespace EvaWeek1
{
    internal class Program
    {
        static int Main(string[] args)
        {
            string filePath;
            do
            {
                filePath = Console.ReadLine() ?? "";

            } while (Path.GetExtension(filePath) != ".txt" || !Path.Exists(filePath));

            DocumentStatistics stat = new DocumentStatistics(filePath);

            try
            {
                stat.Load();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Failed to load file. Message: {ex.Message}");
                return 1;
            }

            return 0;
        }
    }
}