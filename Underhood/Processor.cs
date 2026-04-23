namespace FinalSP.Underhood
{   class Processor
    {
        public static (string content, int replacements, Dictionary<string, int> stats) Process(string filePath, List<string> forbiddenWords)
        {
            string content = File.ReadAllText(filePath);

            int replacements = 0;
            Dictionary<string, int> stats = new Dictionary<string, int>();

            foreach (var word in forbiddenWords)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                int count = 0;
                int index = 0;

                while ((index = content.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    App.Instance.WaitIfPaused();

                    count++;
                    index += word.Length;
                }

                if (count > 0)
                {
                    replacements += count;

                    if (stats.ContainsKey(word))
                        stats[word] += count;
                    else
                        stats[word] = count;

                    content = ReplaceIgnoreCase(content, word, "*******");
                }
            }

            return (content, replacements, stats);
        }

        private static string ReplaceIgnoreCase(string text, string oldValue, string newValue)
        {
            int index = 0;

            while ((index = text.IndexOf(oldValue, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                App.Instance.WaitIfPaused();

                text = text.Remove(index, oldValue.Length).Insert(index, newValue);
                index += newValue.Length;
            }

            return text;
        }
    }
}
