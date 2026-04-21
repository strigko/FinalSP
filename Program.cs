using FinalSP.Underhood;

namespace FinalSP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var words = new List<string> { "bad", "hack" };

            var result = Processor.Process("test.txt", words);

            Console.WriteLine(result.replacements);

            foreach (var w in result.stats)
                Console.WriteLine($"{w.Key}: {w.Value}");

            File.WriteAllText("cleaned.txt", result.content);

            Console.WriteLine("52525252");
        }
    }
}
