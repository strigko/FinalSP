namespace FinalSP.Underhood
{
    class Scanner
    {
        public static void ScanFolder(string folderPath, List<string> forbiddenWords, CancellationToken token)
        {
            string outputDir = Path.Combine(folderPath, "Output");

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(f => !f.Contains("Output"))
                .ToList();

            App.Instance.SetTotal(files.Count);

            Report report = new Report();

            foreach (var file in files)
            {
                App.Instance.WaitIfPaused();

                if (file.Contains("Output"))
                    continue;

                try
                {
                    App.Instance.WaitIfPaused();

                    if (App.Instance.IsStopped())
                        return;

                    var result = Processor.Process(file, forbiddenWords);

                    Thread.Sleep(1000);

                    if (result.replacements > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        string ext = Path.GetExtension(file);

                        string originalPath = Path.Combine(outputDir, fileName + "Original" + ext);
                        string cleanedPath = Path.Combine(outputDir, fileName + "Cleaned" + ext);

                        File.Copy(file, originalPath, true);
                        File.WriteAllText(cleanedPath, result.content);

                        report.AddFile(file, result.replacements);
                        report.AddWordStats(result.stats);

                        Console.WriteLine($"{file} -> found [{result.replacements}]");
                    }
                    else
                    {
                        Console.WriteLine($"{file} -> clean file");
                    }

                    App.Instance.IncreaseProgress();
                }
                catch
                {
                    Console.WriteLine($"Error -> {file}");
                }
            }

            report.Save(Path.Combine(folderPath, "Output", "report.txt"));
            Console.WriteLine("Report saved successfully!");
        }
    }
}