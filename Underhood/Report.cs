namespace FinalSP.Underhood
{
    class Report
    {
        private List<string> files = new List<string>();
        private Dictionary<string, int> wordStats = new Dictionary<string, int>();

        public void AddFile(string filePath, int replacements)
        {
            long size = new FileInfo(filePath).Length;

            files.Add($"{filePath} | size: {size} bytes | replacements: {replacements}");
        }

        public void AddWordStats(Dictionary<string, int> stats)
        {
            foreach (var pair in stats)
            {
                if (wordStats.ContainsKey(pair.Key))
                    wordStats[pair.Key] += pair.Value;
                else
                    wordStats[pair.Key] = pair.Value;
            }
        }

        public void Save(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("||| Files |||");

                foreach (var f in files)
                    sw.WriteLine(f);

                sw.WriteLine();
                sw.WriteLine("||| Top 10 Words |||");

                var top = wordStats
                    .OrderByDescending(x => x.Value)
                    .Take(10);

                foreach (var w in top)
                    sw.WriteLine($"{w.Key}: {w.Value}");
            }
        }
    }
}